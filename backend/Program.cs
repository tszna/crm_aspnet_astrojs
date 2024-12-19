using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using backend.Data;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure DbContext with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Authentication
var jwtSecret = builder.Configuration["JWT:Secret"]
    ?? throw new InvalidOperationException("JWT:Secret not found in configuration.");
var jwtIssuer = builder.Configuration["JWT:ValidIssuer"]
    ?? throw new InvalidOperationException("JWT:ValidIssuer not found in configuration.");
var jwtAudience = builder.Configuration["JWT:ValidAudience"]
    ?? throw new InvalidOperationException("JWT:ValidAudience not found in configuration.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAstro",
        builder =>
        {
            builder.WithOrigins("http://localhost:4321")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

var app = builder.Build();

app.UseHttpsRedirection();

// Use CORS before auth
app.UseCors("AllowAstro");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
