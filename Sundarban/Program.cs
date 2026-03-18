using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Middleware;
using Sundarban.Modules.Customers.Infrastructure;
using Sundarban.Modules.Customers.Presentation;
using Sundarban.Modules.Orders.Infrastructure;
using Sundarban.Modules.Orders.Presentation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<OrdersDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<CustomersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ICustomerService, CustomerService>();

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

app.UseHttpsRedirection();

app.Run();
