using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CinemaApi.Data; 

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// JWT
var key = Encoding.UTF8.GetBytes("MinhaChaveJWTSuperSeguraComMaisDe32CaracteresParaFuncionarCorretamente123456789");

//  Banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    
//  CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

//  Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //  Define esquema JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer SEU_TOKEN"
    });

    //  Aplica segurança nos endpoints
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
            new string[] {}
        }
    });
});

// Autenticação JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

//  Swagger
app.UseSwagger();
app.UseSwaggerUI();

//  CORS
app.UseCors("PermitirTudo");

//  JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();