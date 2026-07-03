using System.Text.Json.Serialization;

namespace R5.Core.Config;

/// <summary>Entorno de WebApiRW contra el que trabaja la app.</summary>
public enum EntornoApi
{
    Produccion,
    Beta,
    Desarrollo,
    Local,
    Personalizado
}

/// <summary>
/// Configuración del dispositivo, persistida en JSON en el directorio de datos de la app.
/// La URL de la API se deriva del entorno elegido: nada hardcodeado en las llamadas.
/// </summary>
public sealed class ConfiguracionApp
{
    public EntornoApi Entorno { get; set; } = EntornoApi.Produccion;

    /// <summary>URL base cuando Entorno = Personalizado (debe terminar en /api).</summary>
    public string UrlPersonalizada { get; set; } = "";

    /// <summary>Número de terminal del dispositivo (1..254).</summary>
    public short NumTerminal { get; set; }

    /// <summary>IdHard del dispositivo enrolado (0 = sin enrolar). La api-key va en el almacén seguro.</summary>
    public int DeviceId { get; set; }

    /// <summary>Último usuario que inició sesión, para precargar el login.</summary>
    public short UltimoUsuario { get; set; }

    [JsonIgnore]
    public string UrlApi => Entorno switch
    {
        EntornoApi.Produccion => "https://webapirw.hiperc.es/webapirw4/api",
        EntornoApi.Beta => "https://webapirw.hiperc.es/webapirw4beta/api",
        EntornoApi.Desarrollo => "https://webapidesarrollo.hiperc.es/api",
        EntornoApi.Local => "https://localhost:7049/api",
        EntornoApi.Personalizado => UrlPersonalizada,
        _ => ""
    };

    public void CopiarDe(ConfiguracionApp otra)
    {
        Entorno = otra.Entorno;
        UrlPersonalizada = otra.UrlPersonalizada;
        NumTerminal = otra.NumTerminal;
        DeviceId = otra.DeviceId;
        UltimoUsuario = otra.UltimoUsuario;
    }
}
