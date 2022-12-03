using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;
using PersistenceLayer.Database;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject repository
builder.Services.AddScoped<IRepository, Repository>();

// inject context
var cs = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddDbContext<IContext, Context>(options => options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// inject mediator
var assembly = Assembly.Load("DomainLayer");
builder.Services.AddMediatR(assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
