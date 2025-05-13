global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using STGeorgeReservation.Data;
using STGeorgeReservation.Helpers;
using STGeorgeReservation.Models;
using STGeorgeReservation.Constants;
using MediatR;

using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using STGeorgeReservation.BaseTypes;
using Microsoft.Extensions.Options;
using STGeorgeReservation.Contracts.Interfaces.Buildings;
using STGeorgeReservation.Contracts.Services;
using STGeorgeReservation.Contracts.Interfaces.Users;
using STGeorgeReservation.Contracts.Interfaces.Reservations;
using HRCom.Utilities.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "SharedResource");


// Other services registration
builder.Services.Configure<JWT>(options => builder.Configuration.GetSection("JWT").Bind(options));
builder.Services.AddIdentity<ApplicationUser, aspnetroles>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<aspnetroles>>();

builder.Services.Configure<STGeorgeReservationSettings>(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString(STGeorgeReservation.Constants.Constants.DefaultDbConnectionName);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString), // Automatically detect server version
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(3); // Retry on failure
            mysqlOptions.CommandTimeout(30); // Set command timeout
        }).EnableDetailedErrors(); // Enable detailed errors

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging(); // Log sensitive data during development
});

builder.Services.AddControllers();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromMinutes(1));


builder.Services.AddScoped<IBuildings, BuildingsService>();
//builder.Services.AddScoped<IAuth, >();
builder.Services.AddScoped<IReservations, ReservationsService>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<IUserDataProvider, UserDataProvider>();

//builder.Services.AddMediatR(BLAssemblyReference.Assembly);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero // Prevent additional leeway for token expiration
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {

            // Skip the default behavior
            context.HandleResponse();
            var tokenLifespan = builder.Services.BuildServiceProvider()
                                          .GetRequiredService<IOptions<DataProtectionTokenProviderOptions>>()
                                          .Value.TokenLifespan;
            // Respond with a custom message
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var response = new { error = new { message = "This Token Is Expired", tokenLifespan = tokenLifespan } };
            await context.Response.WriteAsJsonAsync(response);
        }
    };
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SytemAdmin", policyBuilder =>
    {
        policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                     .RequireAuthenticatedUser();
    });
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Load configuration from environment-specific files
//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables();

//// Example: Reading a connection string
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Console.WriteLine($"Connection String: {connectionString}");



// Configure the middleware pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "STGeorge Apis");
        c.DocumentTitle = "STGeorge API Documentation";
    });
}
app.UseCors();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // Authenticate requests
app.UseAuthorization(); // Apply authorization policies
app.MapControllers();
app.Run();