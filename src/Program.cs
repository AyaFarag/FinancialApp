using FinancialApp.API.Configration;
using FinancialApp.API.Middleware;
using FinancialApp.Application.Automapper;
using FinancialApp.Application.Configration;
using FinancialApp.Application.DTOs;
using FinancialApp.Application.Handlers;
using FinancialApp.Application.Services;
using FinancialApp.Application.Validators;
using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Configration;
using FinancialApp.Infrastructure.Persistence.Data;
using FinancialApp.Infrastructure.Persistence.Seeding;
using FinancialApp.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddApplicationService();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddInfrastructureService();


builder.Services.AddHttpContextAccessor(); 
builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
//    DbInitializer.Seed(context);
//}

app.MapControllers();

app.Run();
