using R5.Core.Api;

namespace R5.Core.Servicios;

/// <summary>
/// Comentarios de documentos (predefinidos + texto libre) contra WebApiRW.
/// tipoDoc/tabla usan los mismos códigos que R3: 1 motivos de rechazo, 2 línea de
/// albarán, 3 cabecera de albarán, 5 traspasos, 6 cabecera de ajuste, 7 línea de ajuste.
/// </summary>
public sealed class ComentarioService
{
    private readonly ApiWeb _api;

    public ComentarioService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET ComentariosPredefinidos/{tipoDoc} — catálogo de comentarios del tipo.</summary>
    public Task<ApiRespuesta<List<string>>> GetPredefinidosAsync(byte tipoDoc, CancellationToken ct = default)
        => _api.GetAsync<List<string>>($"ComentariosPredefinidos/{tipoDoc}", ct);

    /// <summary>POST Comentario/{tabla}/{fecha}/{numDoc}/{lineaDoc}/{codUsuario} — graba el comentario.</summary>
    public Task<ApiRespuesta> PostAsync(byte tabla, DateTime fecha, int numDoc, int lineaDoc, short codUsuario, string comentario, CancellationToken ct = default)
        => _api.PostAsync($"Comentario/{tabla}/{fecha:yyyy-MM-dd}/{numDoc}/{lineaDoc}/{codUsuario}", comentario, ct);
}
