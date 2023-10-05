using System.Text;
using JWT.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  
    // c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    // {
    //     In = ParameterLocation.Header,
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.ApiKey
    // });

    
    
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Description = "JWT Authorization header using the Bearer scheme.",
    //     Type = SecuritySchemeType.Http,
    //     Scheme = "bearer"
    // });
    
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// builder.Services.AddAuthentication().AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         ValidateAudience = false,
//         ValidateIssuer = false,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//             builder.Configuration.GetSection("AppSettings:Token").Value!))
//     };
// });

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
policy =>
{
    policy.WithOrigins("http://localhost:5220/").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITokenManager, TokenManager>()
    .AddScoped<IPasswordManager, PasswordManager>()
    .AddScoped<IUserManager, UserManager>();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// app.MapGet("/security/getMessage",[Authorize]
// () => "Hello World!");
//
// app.MapPost("/security/createToken",
// [AllowAnonymous] (UserDto user) =>
// {
//     if (user is { Username: "Furqat", Password: "furqat123" })
//     {
//         var issuer = builder.Configuration["Jwt:Issuer"];
//         var audience = builder.Configuration["Jwt:Audience"];
//         var key = Encoding.ASCII.GetBytes
//         (builder.Configuration["Jwt:Key"]!);
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(new[]
//             {
//                 new Claim("Id", Guid.NewGuid().ToString()),
//                 new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//                 new Claim(JwtRegisteredClaimNames.Email, user.Username),
//                 new Claim(JwtRegisteredClaimNames.Jti,
//                 Guid.NewGuid().ToString())
//              }),
//             Expires = DateTime.UtcNow.AddSeconds(10),
//             Issuer = issuer,
//             Audience = audience,
//             SigningCredentials = new SigningCredentials
//             (new SymmetricSecurityKey(key),
//             SecurityAlgorithms.HmacSha512Signature)
//         };
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         var jwtToken = tokenHandler.WriteToken(token);
//         var stringToken = tokenHandler.WriteToken(token);
//         return Results.Ok(stringToken);
//     }
//     return Results.Unauthorized();
// });

app.Run();
