using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using R5.Core.Config;

namespace R5.Core.Api;

/// <summary>
/// Wrapper de HttpClient contra WebApiRW (mismo papel que ApiWeb en RWR4, modernizado):
/// compone la URL absoluta desde la configuración en cada llamada (la URL de API puede
/// cambiar en Ajustes sin reiniciar), deserializa JSON, centraliza errores y log.
/// Los servicios usan rutas RELATIVAS ("Usuario/1?pwd=..").
/// </summary>
public sealed class ApiWeb
{
    private readonly HttpClient _http;
    private readonly ConfiguracionApp _config;
    private readonly ILogger<ApiWeb> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public ApiWeb(HttpClient http, ConfiguracionApp config, ILogger<ApiWeb> logger)
    {
        _http = http;
        _config = config;
        _logger = logger;
    }

    public async Task<ApiRespuesta<T>> GetAsync<T>(string rutaRelativa, CancellationToken ct = default)
    {
        // GET es idempotente: un reintento automático ante fallo de red.
        var respuesta = await EnviarAsync<T>(() => new HttpRequestMessage(HttpMethod.Get, Url(rutaRelativa)), ct);
        if (!respuesta.Ok && respuesta.SinConexion && !ct.IsCancellationRequested)
        {
            respuesta = await EnviarAsync<T>(() => new HttpRequestMessage(HttpMethod.Get, Url(rutaRelativa)), ct);
        }
        return respuesta;
    }

    /// <summary>GET de contenido binario (PDF de informes, etc.).</summary>
    public async Task<ApiRespuesta<byte[]>> GetBytesAsync(string rutaRelativa, CancellationToken ct = default)
    {
        string url = "";
        try
        {
            url = Url(rutaRelativa);
            using var respuesta = await _http.GetAsync(url, ct);
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                byte[] datos = await respuesta.Content.ReadAsByteArrayAsync(ct);
                return ApiRespuesta<byte[]>.Exito(datos, respuesta.StatusCode);
            }
            string error = await LeerErrorAsync(respuesta, url, ct);
            return ApiRespuesta<byte[]>.Fallo(error, respuesta.StatusCode);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return ApiRespuesta<byte[]>.Fallo("Operación cancelada.");
        }
        catch (Exception ex) when (EsFalloDeRed(ex))
        {
            _logger.LogError(ex, "Sin conexión {Url}", url);
            return ApiRespuesta<byte[]>.Fallo("No hay conexión con el servidor.", sinConexion: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado {Url}", url);
            return ApiRespuesta<byte[]>.Fallo("Error inesperado: " + ex.Message);
        }
    }

    public Task<ApiRespuesta<T>> PostAsync<T>(string rutaRelativa, object? cuerpo, CancellationToken ct = default)
        => EnviarAsync<T>(() => new HttpRequestMessage(HttpMethod.Post, Url(rutaRelativa))
        {
            Content = JsonContent.Create(cuerpo, options: _jsonOptions)
        }, ct);

    public Task<ApiRespuesta> PostAsync(string rutaRelativa, object? cuerpo, CancellationToken ct = default)
        => EnviarSinCuerpoAsync(() => new HttpRequestMessage(HttpMethod.Post, Url(rutaRelativa))
        {
            Content = JsonContent.Create(cuerpo, options: _jsonOptions)
        }, ct);

    public Task<ApiRespuesta<T>> PutAsync<T>(string rutaRelativa, object? cuerpo, CancellationToken ct = default)
        => EnviarAsync<T>(() => new HttpRequestMessage(HttpMethod.Put, Url(rutaRelativa))
        {
            Content = JsonContent.Create(cuerpo, options: _jsonOptions)
        }, ct);

    public Task<ApiRespuesta> DeleteAsync(string rutaRelativa, CancellationToken ct = default)
        => EnviarSinCuerpoAsync(() => new HttpRequestMessage(HttpMethod.Delete, Url(rutaRelativa)), ct);

    private string Url(string rutaRelativa)
    {
        string baseUrl = _config.UrlApi;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("URL de la API no configurada. Ve a Ajustes.");
        }
        return baseUrl.TrimEnd('/') + "/" + rutaRelativa.TrimStart('/');
    }

    private async Task<ApiRespuesta<T>> EnviarAsync<T>(Func<HttpRequestMessage> crearPeticion, CancellationToken ct)
    {
        string url = "";
        try
        {
            using var peticion = crearPeticion();
            url = peticion.RequestUri!.ToString();
            using var respuesta = await _http.SendAsync(peticion, ct);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var datos = await respuesta.Content.ReadFromJsonAsync<T>(_jsonOptions, ct);
                if (datos is null)
                {
                    _logger.LogWarning("Respuesta JSON nula {Url}", url);
                    return ApiRespuesta<T>.Fallo("Respuesta vacía del servidor.", respuesta.StatusCode);
                }
                return ApiRespuesta<T>.Exito(datos, respuesta.StatusCode);
            }

            string error = await LeerErrorAsync(respuesta, url, ct);
            return ApiRespuesta<T>.Fallo(error, respuesta.StatusCode);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return ApiRespuesta<T>.Fallo("Operación cancelada.");
        }
        catch (Exception ex) when (EsFalloDeRed(ex))
        {
            _logger.LogError(ex, "Sin conexión {Url}", url);
            return ApiRespuesta<T>.Fallo("No hay conexión con el servidor.", sinConexion: true);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON inválido {Url}", url);
            return ApiRespuesta<T>.Fallo("Respuesta del servidor no válida.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado {Url}", url);
            return ApiRespuesta<T>.Fallo("Error inesperado: " + ex.Message);
        }
    }

    private async Task<ApiRespuesta> EnviarSinCuerpoAsync(Func<HttpRequestMessage> crearPeticion, CancellationToken ct)
    {
        string url = "";
        try
        {
            using var peticion = crearPeticion();
            url = peticion.RequestUri!.ToString();
            using var respuesta = await _http.SendAsync(peticion, ct);

            if (respuesta.IsSuccessStatusCode)
            {
                return ApiRespuesta.Exito(respuesta.StatusCode);
            }
            string error = await LeerErrorAsync(respuesta, url, ct);
            return ApiRespuesta.Fallo(error, respuesta.StatusCode);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return ApiRespuesta.Fallo("Operación cancelada.");
        }
        catch (Exception ex) when (EsFalloDeRed(ex))
        {
            _logger.LogError(ex, "Sin conexión {Url}", url);
            return ApiRespuesta.Fallo("No hay conexión con el servidor.", sinConexion: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado {Url}", url);
            return ApiRespuesta.Fallo("Error inesperado: " + ex.Message);
        }
    }

    private async Task<string> LeerErrorAsync(HttpResponseMessage respuesta, string url, CancellationToken ct)
    {
        string detalle = "";
        try
        {
            detalle = await respuesta.Content.ReadAsStringAsync(ct);
        }
        catch { /* cuerpo ilegible: se informa solo el status */ }

        string error = respuesta.StatusCode switch
        {
            HttpStatusCode.BadRequest => string.IsNullOrWhiteSpace(detalle) ? "Petición incorrecta." : detalle,
            HttpStatusCode.Unauthorized => "Dispositivo no autorizado. Revisa el enrolamiento en Ajustes.",
            HttpStatusCode.NotFound => "No encontrado.",
            _ => $"{(int)respuesta.StatusCode} {respuesta.ReasonPhrase}" + (string.IsNullOrWhiteSpace(detalle) ? "" : $": {Recortar(detalle)}")
        };

        _logger.LogWarning("{Codigo} {Url} {Detalle}", (int)respuesta.StatusCode, url, Recortar(detalle));
        return error;
    }

    private static string Recortar(string texto) => texto.Length <= 300 ? texto : texto[..300];

    // Timeout (TaskCanceled sin ct externa) y errores de socket cuentan como fallo de red.
    private static bool EsFalloDeRed(Exception ex)
        => ex is HttpRequestException or TaskCanceledException or TimeoutException;
}
