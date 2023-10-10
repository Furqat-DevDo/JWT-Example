using System.Text;
using JWT_advanced.Options;
using JWT_advanced.Services;
using JWT_advanced.Services.Interfaces;
using JWT.Data;
using JWT.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("Db");

builder.Services.AddAuthentication(b =>
{
    b.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    b.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    b.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(j =>
    {
        j.TokenValidationParameters = new TokenValidationParameters()
        {
            // Tekshirish qismi
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            
            // Ruxsat berilgan qiymatlar ro'yxati 
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        };
    });

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdminOnly", d => {d.RequireRole("Admin").RequireClaim("Id");});  
});

builder.Services.AddCors(options => options.AddPolicy(name: "OurWhiteList",
policy =>
{
    // policy.WithOrigins("https://ilmhub.uz/").AllowAnyMethod().AllowAnyHeader();
    // policy.WithOrigins("http://localhost:5258").AllowAnyMethod().AllowAnyHeader();
    policy.AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITokenManager, TokenManager>()
    .AddScoped<IPasswordManager, PasswordManager>()
    .AddScoped<IUserManager, UserManager>();

builder.Services.AddControllers();

builder.Services.AddRouting(o => { o.LowercaseUrls = true;});

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlite(connection);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
