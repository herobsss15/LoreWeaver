using LoreWeaver.Components;
using LoreWeaver.Data;
using LoreWeaver.Features.Rules;
using Microsoft.EntityFrameworkCore;

namespace LoreWeaver;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // AddDbContextFactory, not AddDbContext: Blazor Server components can
        // outlive a request, so each operation gets its own short-lived context.
        builder.Services.AddDbContextFactory<LoreWeaverDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpClient<SrdClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Srd:BaseUrl"] ?? "https://www.dnd5eapi.co");
        });
        builder.Services.AddSingleton<RulesQueryService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
