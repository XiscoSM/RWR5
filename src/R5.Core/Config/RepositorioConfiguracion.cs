using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace R5.Core.Config;

/// <summary>
/// Carga/guarda la configuración en config.json dentro del directorio de datos
/// de la app (la ruta la aporta el host MAUI; Core no conoce la plataforma).
/// </summary>
public sealed class RepositorioConfiguracion
{
    private readonly string _rutaArchivo;
    private readonly ILogger<RepositorioConfiguracion> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public RepositorioConfiguracion(string directorioDatos, ILogger<RepositorioConfiguracion> logger)
    {
        _rutaArchivo = Path.Combine(directorioDatos, "config.json");
        _logger = logger;
    }

    /// <summary>Rellena la instancia con lo persistido. Si no hay archivo o está corrupto, deja los valores por defecto.</summary>
    public void Cargar(ConfiguracionApp config)
    {
        try
        {
            if (!File.Exists(_rutaArchivo)) return;

            string json = File.ReadAllText(_rutaArchivo);
            var leida = JsonSerializer.Deserialize<ConfiguracionApp>(json, _jsonOptions);
            if (leida is not null) config.CopiarDe(leida);
        }
        catch (Exception ex)
        {
            // Configuración corrupta no debe impedir arrancar: se sigue con defaults.
            _logger.LogError(ex, "Error leyendo {Ruta}", _rutaArchivo);
        }
    }

    public void Guardar(ConfiguracionApp config)
    {
        string json = JsonSerializer.Serialize(config, _jsonOptions);
        File.WriteAllText(_rutaArchivo, json);
    }
}
