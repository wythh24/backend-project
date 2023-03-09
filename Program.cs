using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Org.BouncyCastle.Crypto.Tls;
using productstockingv1.Data;
using productstockingv1.models;
using productstockingv1.Interfaces;
using productstockingv1.Repository;
using productstockingv1.Models.Request;
using productstockingv1.Validation;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//modified using statement in (appsettings.json)
// var config = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

//ignore cycle
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

const string policy = "myPolicy";

//Enable cors
builder.Services.AddCors(options =>
    {
        options.AddPolicy(policy,
            build => { build.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });
    }
);


//comment before modify
builder.Services.AddDbContext<ProductContext>(
    options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("Development"),
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
    });

/*builder.Services.AddDbContext<ProductContext>(
    options => { options.UseSqlServer(builder.Configuration.GetConnectionString("Development")); },
    ServiceLifetime.Transient
);*/

//add DbContext
builder.Services.AddDbContext<IProductContext, ProductContext>();

//add scope
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<Product, string>, ProductRepository>();
builder.Services.AddScoped<IRepository<Ware, string>, WareRepository>();
builder.Services.AddScoped<IRepository<Stocking, string>, StockingRepository>();

//add validation
builder.Services.AddScoped<IValidator<ProductCreateReq>, ProductValidate>();
builder.Services.AddScoped<IValidator<WareCreateReq>, WareValidate>();

builder.Services.AddFluentValidation();

// add auto mapper
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//check is enable
app.UseCors(policy);

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