using DotNetApi.Config;
using DotNetApi.Models;
using DotNetApi.Repositories;
using DotNetApi.Repositories.EntityFramework;
using DotNetApi.Services;
using DotNetApi.Helpers.Emails;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using DotNetApi.Helpers.Tokens;
using DotNetApi.Helpers.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// ++++++++++++++++++++++++++++++
// Add services to the container.
// ++++++++++++++++++++++++++++++

// Database for EF
builder.Services.AddDbContext<WhateverContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection")!
    );
});

// Use database stored Microsoft Identity for register and login
builder.Services.AddIdentity<UserAccount, UserRole>()
    .AddEntityFrameworkStores<WhateverContext>()
    .AddDefaultTokenProviders();

// Configure Microsoft Identity
// see : https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0
builder.Services.Configure<IdentityOptions>(options =>
{
    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    // Password settings
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = ValidationConfig.PasswordMinimalLength;
    options.Password.RequiredUniqueChars = 4;
    // SignIn settings
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // Default lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

// Configure JWT authentication
JwtSettings jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidateIssuer = true,
        ValidAudience = jwtSettings.Audience,
        ValidateAudience = true,
        IssuerSigningKey = jwtSettings.SecretSymmetricSecurityKey,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});

// Dependency injection for app settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWTSettings"));
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("FrontendSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// API controllers
builder.Services.AddControllers(options =>
{
    // Require authenticated user everywhere
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Fluent validation
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});

// Fluent validators
builder.Services.AddScoped<IValidator<RegisterModel>, RegisterModelValidator>();
builder.Services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();
builder.Services.AddScoped<IValidator<ForgottenPasswordModel>, ForgottenPasswordModelValidator>();
builder.Services.AddScoped<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();
builder.Services.AddScoped<IValidator<UserAccountDTO>, UserAccountDTOValidator>();
builder.Services.AddScoped<IValidator<WhateverDTO>, WhateverDTOValidator>();

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

// Dependency injection for ClaimsPrincipal user
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(provider => provider.GetService<IHttpContextAccessor>()!.HttpContext!.User);

// Dependency injection for IUrlHelper
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x => {
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext!);
});

// Custom services
builder.Services.AddTransient<ITokenHelpers, TokenHelpers>();
builder.Services.AddTransient<IEmailService, MailKitEmailService>();
builder.Services.AddTransient<IEmailHelpers, EmailHelpers>();
// Database
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IUserAccountService, UserAccountService>();
builder.Services.AddTransient<IWhateverService, WhateverService>();

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

app.UseWhateverException();

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
