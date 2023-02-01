using DotNetApi.Config;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Models;
using DotNetApi.Repositories;
using DotNetApi.Repositories.EntityFramework;
using DotNetApi.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ++++++++++++++++++++++++++++++
// Add services to the container.
// ++++++++++++++++++++++++++++++

// Database for EF
builder.Services.AddDbContext<VesselContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection")!
    );
});

// Dependency injection for app settings
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("FrontendSettings"));

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// API controllers
builder.Services.AddControllers();

// Fluent validation
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});

// Fluent validators
builder.Services.AddScoped<IValidator<VesselDTO>, VesselDTOValidator>();
builder.Services.AddScoped<IValidator<PositionDTO>, PositionDTOValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WhateverDotNetApi", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization using the Bearer scheme.\r\n\r\nEnter 'Bearer <your_token>' in the text input below.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Custom services
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IVesselService, VesselService>();
builder.Services.AddTransient<IPositionService, PositionService>();

// Serilog
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// ++++++++++++++++++++++++++++++++++++
// Configure the HTTP request pipeline.
// ++++++++++++++++++++++++++++++++++++

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseVesselRoutesDemoException();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Enable CORS 
app.UseCors(policy =>
    policy
    .WithOrigins(builder.Configuration.GetSection("FrontendSettings")["Url"]!)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.MapControllers();

app.Run();
