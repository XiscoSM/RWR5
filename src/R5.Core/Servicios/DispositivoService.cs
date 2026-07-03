using Microsoft.Extensions.Logging;
using R5.Core.Api;
using R5.Core.Config;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// Identidad del dispositivo frente a WebApiRW: verificación de la API (ApiInfo),
/// enrolamiento (canje único de api-key) y carga de credenciales al arrancar.
/// </summary>
public sealed class DispositivoService
{
    private readonly ApiWeb _api;
    private readonly ConfiguracionApp _config;
    private readonly RepositorioConfiguracion _repoConfig;
    private readonly CredencialesApi _credenciales;
    private readonly IAlmacenSecretos _secretos;
    private readonly ILogger<DispositivoService> _logger;

    public DispositivoService(
        ApiWeb api,
        ConfiguracionApp config,
        RepositorioConfiguracion repoConfig,
        CredencialesApi credenciales,
        IAlmacenSecretos secretos,
        ILogger<DispositivoService> logger)
    {
        _api = api;
        _config = config;
        _repoConfig = repoConfig;
        _credenciales = credenciales;
        _secretos = secretos;
        _logger = logger;
    }

    /// <summary>Carga en memoria la api-key guardada. Llamar una vez al arrancar.</summary>
    public async Task CargarCredencialesAsync()
    {
        try
        {
            if (_config.DeviceId <= 0) return;
            string? apiKey = await _secretos.ObtenerAsync(ClavesSecretos.ApiKey);
            _credenciales.Establecer(_config.DeviceId, apiKey);
        }
        catch (Exception ex)
        {
            // Sin credenciales se puede seguir: la UI pedirá enrolar de nuevo.
            _logger.LogError(ex, "Error cargando credenciales del dispositivo");
        }
    }

    /// <summary>GET ApiInfo (anónimo) — comprueba que la URL configurada responde y qué entorno hay detrás.</summary>
    public Task<ApiRespuesta<ApiInfo>> ProbarApiAsync(CancellationToken ct = default)
        => _api.GetAsync<ApiInfo>("ApiInfo", ct);

    /// <summary>
    /// POST ApiCliente/Enrolar/{idHard} — canje de un solo uso: la API devuelve la api-key
    /// en claro UNA vez; se guarda en el almacén seguro y queda activa en memoria.
    /// </summary>
    public async Task<ApiRespuesta> EnrolarAsync(int idHard, CancellationToken ct = default)
    {
        var respuesta = await _api.PostAsync<RespuestaEnrolar>($"ApiCliente/Enrolar/{idHard}", cuerpo: null, ct);
        if (!respuesta.Ok || respuesta.Datos is null)
        {
            return ApiRespuesta.Fallo(respuesta.Error, respuesta.Codigo, respuesta.SinConexion);
        }

        await _secretos.GuardarAsync(ClavesSecretos.ApiKey, respuesta.Datos.ApiKey);
        _credenciales.Establecer(respuesta.Datos.IdHard, respuesta.Datos.ApiKey);
        _config.DeviceId = respuesta.Datos.IdHard;
        _repoConfig.Guardar(_config);

        return ApiRespuesta.Exito(respuesta.Codigo!.Value);
    }

    private sealed class RespuestaEnrolar
    {
        public int IdHard { get; set; }
        public string ApiKey { get; set; } = "";
    }
}
