using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Middleware;
using Sundarban.Modules.Customers.Infrastructure;
using Sundarban.Modules.Customers.Presentation;
using Sundarban.Modules.Orders.Infrastructure;
using Sundarban.Modules.Orders.Presentation;
using Scalar.AspNetCore;
using Sundarban.Modules.Payments.Infrastructure;
using Sundarban.Modules.Payments.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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

app.UseHttpsRedirection();

app.Run();
