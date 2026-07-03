using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Recepción de albaranes de proveedor contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class AlbaranService
{
    private readonly ApiWeb _api;

    public AlbaranService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Albaran/List/{codAlm}?codUsuario=&amp;registrado=&amp;top= — albaranes del usuario.</summary>
    public Task<ApiRespuesta<List<Albaran>>> GetAlbaranesAsync(short codAlm, short codUsuario, bool registrado, short top = 50, CancellationToken ct = default)
        => _api.GetAsync<List<Albaran>>($"Albaran/List/{codAlm}?codUsuario={codUsuario}&registrado={registrado}&top={top}", ct);

    /// <summary>GET Albaran/{num}/{fecha}/Lineas — detalle del albarán.</summary>
    public Task<ApiRespuesta<List<AlbaranLin>>> GetLineasAsync(int numAlbaran, DateTime fecha, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<AlbaranLin>>($"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}/Lineas?top={top}", ct);
}
