using LoreWeaver.Application.Implementations;
using LoreWeaver.Application.Services;
using LoreWeaver.Repository.Data;
using LoreWeaver.Repository.Implementations;
using LoreWeaver.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<LoreWeaverContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMundoRepository, MundoRepository>();
        services.AddScoped<MundoService>();

        services.AddScoped<IEventoRepository, EventoRepository>();
        services.AddScoped<EventoService>();

        services.AddScoped<ILugarRepository, LugarRepository>();
        services.AddScoped<LugarService>();

        services.AddScoped<IPersonagemRepository, PersonagemRepository>();
        services.AddScoped<PersonagemService>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<UsuarioService>();

        services.AddScoped<IVersaoRepository, VersaoRepository>();
        services.AddScoped<VersaoService>();

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoreWeaver.API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoreWeaver.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}