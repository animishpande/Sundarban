using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Middleware;
using Sundarban.Modules.Customers.Infrastructure;
using Sundarban.Modules.Customers.Presentation;
using Sundarban.Modules.Orders.Infrastructure;
using Sundarban.Modules.Orders.Presentation;
using Scalar.AspNetCore;
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
builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<OrderPaidConsumer>();

    m.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-paid-notifications", e =>
        {
            e.ConfigureConsumer<OrderPaidConsumer>(context);
        });
    });
});

// Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "sundarban:"; // all keys prefixed — avoids collisions
});

var app = builder.Build();

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
