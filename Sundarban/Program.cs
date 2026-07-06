using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Middleware;
using Sundarban.Modules.Customers.Infrastructure;
using Sundarban.Modules.Customers.Presentation;
using Sundarban.Modules.Orders.Infrastructure;
using Sundarban.Modules.Orders.Presentation;
using Scalar.AspNetCore;
using Sundarban.Modules.Customers.Domain;
using Sundarban.Modules.Notifications.Consumers;
using Sundarban.Modules.Notifications.Presentation;
using Sundarban.Modules.Payments.Infrastructure;
using Sundarban.Modules.Payments.Presentation;
using Sundarban.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSignalR();

// Schema and Database Configurations
builder.Services.AddDbContext<OrdersDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<CustomersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Interface Configurations
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// MassTransit Configuration
var useRabbitMq = builder.Configuration.GetValue<bool>("Features:UseRabbitMq");
var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");
var rabbitMqHost = rabbitMqSection["Host"] ?? "localhost";
var rabbitMqVirtualHost = rabbitMqSection["VirtualHost"] ?? "/";

builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<OrderPaidConsumer>();
    if (useRabbitMq)
    {
        m.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqHost, rabbitMqVirtualHost, h =>
            {
                var rabbitMqUsername = rabbitMqSection["Username"];
                var rabbitMqPassword = rabbitMqSection["Password"];

                if (!string.IsNullOrEmpty(rabbitMqUsername))
                {
                    h.Username(rabbitMqUsername);
                }

                if (!string.IsNullOrEmpty(rabbitMqPassword))
                {
                    h.Password(rabbitMqPassword);
                }
            });

            cfg.ReceiveEndpoint("order-paid-notifications", e =>
            {
                e.ConfigureConsumer<OrderPaidConsumer>(context);
            });
        });
    } else
    {
        m.UsingInMemory((ctx, cfg) =>
        {
            cfg.ConfigureEndpoints(ctx);
        });
    }
});

// Redis Configuration
var useRedis = builder.Configuration.GetValue<bool>("Features:UseRedis");
if (useRedis)
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        options.InstanceName = "sundarban:"; // all keys prefixed — avoids collisions
    });
} 
else
{
    builder.Services.AddDistributedMemoryCache(); // IDistributedCache backed by memory
}

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await dbContext.Database.MigrateAsync();

    if (!await dbContext.Customers.AnyAsync())
    {
        logger.LogInformation("Seeding customers into database");

        var customers = new[]
        {
            Customer.Create("Seeded", "User", "seededuser@new.com")
        };

        await dbContext.Customers.AddRangeAsync(customers);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Seeded {count} customers", customers.Length);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options.WithTitle("Sundarban API");
    });
}

app.UseMiddleware<ExceptionHandler>();

app.MapOrdersEndpoints();

app.MapCustomerEndpoints();

app.MapPaymentsEndpoints();

app.MapNotificationsEndpoints();

app.UseHttpsRedirection();

app.Run();
