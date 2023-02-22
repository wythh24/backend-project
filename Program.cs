using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using productstockingv1.Data;
using productstockingv1.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductContext>(
               options =>
               {

                   options.UseSqlServer(builder.Configuration.GetConnectionString("Developments"));
               },
               ServiceLifetime.Transient
               );

builder.Services.AddFluentValidation();

// add validation
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();