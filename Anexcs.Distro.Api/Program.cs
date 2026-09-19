using Anexcs.Distro.Application;
using Anexcs.Distro.Infrastructure;
using Api.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Exception Handling Middleware
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        
        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseExceptionHandler(); 
        
        app.MapControllers();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }
}