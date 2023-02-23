using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using productstockingv1.Data;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;
using productstockingv1.Repository;
using productstockingv1.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductContext>(
    options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("Development"),
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
    });
//add dbcontext
builder.Services.AddDbContext<IProductContext, ProductContext>();

//add scope
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<Product, string>, ProductRepository>();
builder.Services.AddScoped<IRepository<Ware, string>, WareRepository>();
builder.Services.AddScoped<IRepository<Stocking, string>, StockingRepository>();


//add validation
builder.Services.AddScoped<IValidator<ProductCreateReq>, ProductValidate>();

builder.Services.AddFluentValidation();

// add mapper
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();