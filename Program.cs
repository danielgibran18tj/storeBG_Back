using System.Text;
using BG.Data;
using BG.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BG.Data.Repository;
using BG.Domain;
using BG.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigin", builder =>
    {
        builder.AllowAnyOrigin()    //.WithOrigins("http://localhost:4200", "http://frontend:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSqlServer<DataBaseContext>(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);

builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ProductService>();

// TODO: buscar la manera de inyectar RsaKeyManagerFile donde se este usando
// instancia para encriptacion de contrasena asimetrica en memoria o File
// var rsaKeyManager = RsaKeyManagerMemory.Instance;
var rsaKeyManager = RsaKeyManagerFile.Instance;

// Agregar servicio de autenticación JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var value = jwtSettings.Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = value.Issuer,
            ValidAudience = value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value.SecretKey))
        };
    });

builder.Services.AddAuthorization(); // Se agrega servicios de autorización
builder.WebHost.UseUrls(builder.Configuration.GetConnectionString("ApiEndpoint") ?? string.Empty);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularOrigin");
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
