using Microsoft.Extensions.Logging;
using R5.App.Servicios;
using R5.Core.Api;
using R5.Core.Config;
using R5.Core.Hardware;
using R5.Core.Logging;
using R5.Core.Servicios;

namespace R5.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Log estructurado a archivo (JSONL con rotación), como en RWR4 pero acotado.
        string archivoLog = Path.Combine(FileSystem.AppDataDirectory, "r5log.jsonl");
        builder.Logging.AddProvider(new FileLoggerProvider(archivoLog, LogLevel.Warning));

        // ---- Configuración y credenciales (singletons: estado del dispositivo) ----
        builder.Services.AddSingleton(sp =>
            new RepositorioConfiguracion(FileSystem.AppDataDirectory,
                sp.GetRequiredService<ILogger<RepositorioConfiguracion>>()));
        builder.Services.AddSingleton(sp =>
        {
            var config = new ConfiguracionApp();
            sp.GetRequiredService<RepositorioConfiguracion>().Cargar(config);
            return config;
        });
        builder.Services.AddSingleton<CredencialesApi>();
        builder.Services.AddSingleton<IAlmacenSecretos, AlmacenSecretosMaui>();
        builder.Services.AddSingleton<IEstadoRed, EstadoRedMaui>();

        // ---- Acceso a WebApiRW: un HttpClient con cabeceras de API key ----
        builder.Services.AddSingleton(sp => new HttpClient(
            new ApiKeyHandler(sp.GetRequiredService<CredencialesApi>())
            {
                InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(5) }
            })
        {
            Timeout = TimeSpan.FromSeconds(15)
        });
        builder.Services.AddSingleton<ApiWeb>();

        // ---- Servicios de negocio ----
        builder.Services.AddSingleton<SesionUsuario>();
        builder.Services.AddSingleton<UsuarioService>();
        builder.Services.AddSingleton<DispositivoService>();
        builder.Services.AddSingleton<ProductoService>();
        builder.Services.AddSingleton<PedidoCentralService>();
        builder.Services.AddSingleton<TraspasoService>();
        builder.Services.AddSingleton<AlbaranService>();
        builder.Services.AddSingleton<PedidoCompraService>();
        builder.Services.AddSingleton<InventarioService>();
        builder.Services.AddSingleton<AjusteService>();
        builder.Services.AddSingleton<PreparacionService>();
        builder.Services.AddSingleton<InformeService>();
        builder.Services.AddSingleton<ReportService>();
        builder.Services.AddSingleton<AlmacenService>();
        builder.Services.AddSingleton<IVisorArchivos, VisorArchivosMaui>();

        // ---- Hardware (impl. real por plataforma en fases posteriores) ----
        builder.Services.AddSingleton<IBalanza, BalanzaNula>();

        var app = builder.Build();

        // Carga la api-key guardada antes de la primera llamada (arranque, sin UI aún).
        app.Services.GetRequiredService<DispositivoService>()
            .CargarCredencialesAsync().GetAwaiter().GetResult();

        return app;
    }
}
