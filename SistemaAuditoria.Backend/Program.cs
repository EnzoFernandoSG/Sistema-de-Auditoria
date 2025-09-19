using SistemaAuditoria.Backend.Configurations;
using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.DTOs;
using SistemaAuditoria.Backend.Services;
using SistemaAuditoria.Backend.Services.Implementations;

using Supabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); //.AddApplicationPart(typeof(AuditoriaController).Assembly); // Se o controller não for descoberto automaticamente

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(new Supabase.Client(
    builder.Configuration["ApiKeys:SupabaseUrl"],
    builder.Configuration["ApiKeys:SupabaseKey"]
));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFranchiseService, FranchiseService>();
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IDealService, DealService>();
builder.Services.AddScoped<INfeService, NfeService>();
builder.Services.AddScoped<IAuditService, AuditService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    //  Swagger se MapOpenApi não estiver funcionando
    //app.UseSwagger();
    //app.UseSwaggerUI();
    // app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection(); 
app.UseRouting(); 

app.UseCors("AllowSpecificOrigin"); 
app.UseAuthorization(); 
app.MapControllers(); 

app.Run();