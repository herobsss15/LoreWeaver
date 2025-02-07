using LoreWeaver.Repository.Implementations;
using LoreWeaver.Repository.Interfaces;
using LoreWeaver.Application.Services;
using LoreWeaver.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LoreWeaver.Application.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços de aplicação
builder.Services.AddScoped<MundoService>();
// builder.Services.AddScoped<EventoService>();
// builder.Services.AddScoped<LugarService>();
builder.Services.AddScoped<PersonagemService>();
// builder.Services.AddScoped<UsuarioService>();
// builder.Services.AddScoped<VersaoService>();

// Adicionar repositórios
builder.Services.AddScoped<IMundoRepository, MundoRepository>();
// builder.Services.AddScoped<IEventoRepository, EventoRepository>();
// builder.Services.AddScoped<ILugarRepository, LugarRepository>();
builder.Services.AddScoped<IPersonagemRepository, PersonagemRepository>();
// builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
// builder.Services.AddScoped<IVersaoRepository, VersaoRepository>();

// Adicionar o serviço de banco de dados
builder.Services.AddDbContext<LoreWeaverContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .SetIsOriginAllowedToAllowWildcardSubdomains()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Adicionar controladores
builder.Services.AddControllers();

// Adicionar autorização
builder.Services.AddAuthorization();

// Adicionar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoreWeaver.API", Version = "v1" });
});

var app = builder.Build();

// Configure o pipeline de solicitação HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5000");