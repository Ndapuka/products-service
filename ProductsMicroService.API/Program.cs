using System;
using eCommerce.ProductsMicroService.API.Middleware;
using eCommerce.ProductsService.BusinessLogicLayer;
using eCommerce.ProductsService.DataAccessLayer;
using FluentValidation;
using FluentValidation.AspNetCore;
using eCommerce.ProductsMicroService.API.APIEndpoints;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
//using eCommerce.BusinessLogicLayer.RabbitMQ;


var builder = WebApplication.CreateBuilder(args);

//docker run -d --name products-microservice -p 3000:3000 ndapuka7/products-microservice:latest

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();


//Add services controllers
builder.Services.AddControllers();


//Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();

//Add model binder to read values from JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add Swagger services
builder.Services.AddSwaggerGen();

// add Cors service  
builder.Services.AddCors(options => { options.AddDefaultPolicy(builder => {
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }); 
});

var app = builder.Build();


//Add ExceptionHandlingMiddleware middleware
app.UseExceptionHandlingMiddleware();
//Add routing
app.UseRouting();

//Cors
app.UseCors();
//Add Swagger
app.UseSwagger();
app.UseSwaggerUI();
//Add Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Add Endpoints
//add controller routing endpoints


app.MapControllers();
app.MapProuctAPIEndpoints();


app.Run();
