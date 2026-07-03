using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Traspasos entre almacenes contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class TraspasoService
{
    private readonly ApiWeb _api;

    public TraspasoService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Traspasos/List/{codAlm}?codUsuario=&amp;registrado=&amp;top= — traspasos del usuario.</summary>
    public Task<ApiRespuesta<List<Traspaso>>> GetTraspasosAsync(short codAlm, short codUsuario, bool registrado, short top = 50, CancellationToken ct = default)
        => _api.GetAsync<List<Traspaso>>($"Traspasos/List/{codAlm}?codUsuario={codUsuario}&registrado={registrado}&top={top}", ct);

    /// <summary>
    /// GET Traspasos/{num}/{fecha}/Lineas — líneas. Sin pedido asociado: numPedido=0 y
    /// fechaPedido=1900-01-01 (el "sin fecha" de la BD; 0001-01-01 revienta SqlDateTime).
    /// </summary>
    public Task<ApiRespuesta<List<TraspasoLin>>> GetLineasAsync(int numTraspaso, DateTime fecha, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<TraspasoLin>>($"Traspasos/{numTraspaso}/{fecha:yyyy-MM-dd}/Lineas?fechaPedido=1900-01-01&numPedido=0&top={top}", ct);
}
