using AdminSystem.API.Middleware;
using AdminSystem.Model;
using AdminSystem.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the controller
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for connecting the database so it can check email and password
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppProgDb")));

builder.Services.AddScoped< CustomerRepository, CustomerRepository>();
builder.Services.AddScoped< EbookRepository, EbookRepository>();
builder.Services.AddScoped< OrderRepository, OrderRepository>();
builder.Services.AddScoped< OrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped< CategoryRepository, CategoryRepository>();

builder.Services.AddScoped<BasicAuthenticationMiddleware>();

Console.WriteLine($"DB Connection: {builder.Configuration.GetConnectionString("AppProgDb")}");

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//app.UseHeaderAuthenticationMiddleware();
//app.UseMiddleware<BasicAuthenticationMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
