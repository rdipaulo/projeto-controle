using BettingControl.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite; // Adicionar para SQLite
// Usings necessários para JWT e Autenticação
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BettingControl.API.Repositories; // Para Repositórios
using BettingControl.API.Services;     // Para Serviços

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------
// 1. CONFIGURAÇÃO DE SERVIÇOS (Injeção de Dependência)
// -----------------------------------------------------------

// Adiciona suporte a Controllers (essencial para AuthController, BetsController, CiclosController)
builder.Services.AddControllers(); 

// Registro do DbContext (Entity Framework Core)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=BettingControl.db"; // Fallback para SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Registro do serviço de senha (já existente)
var pepper = builder.Configuration["PasswordPepper"] ?? Environment.GetEnvironmentVariable("PASSWORD_PEPPER") ?? string.Empty;
builder.Services.AddSingleton<BettingControl.API.Services.IPasswordService>(sp => new BettingControl.API.Services.PasswordService(pepper));

// Registro dos Repositórios
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<ICicloRepository, CicloRepository>();

// Registro dos Serviços (Lógica de Negócio)
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<ICicloService, CicloService>();
builder.Services.AddScoped<IServicoFinanceiro, ServicoFinanceiro>();
builder.Services.AddScoped<IServicoDeAnalise, ServicoDeAnalise>();
builder.Services.AddScoped<IServicoDeFechamentoMensal, ServicoDeFechamentoMensal>();


// Configuração da Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado")))
        };
    });

// Adiciona o serviço de Autorização
builder.Services.AddAuthorization();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// -----------------------------------------------------------
// 2. CONSTRUÇÃO E CONFIGURAÇÃO DO PIPELINE HTTP
// -----------------------------------------------------------

var app = builder.Build();

// Configuração do pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    // Habilita Swagger UI apenas em ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares de Autenticação e Autorização (DEVE vir antes de MapControllers)
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento dos Controllers
app.MapControllers(); 

// Inicia a aplicação
app.Run();