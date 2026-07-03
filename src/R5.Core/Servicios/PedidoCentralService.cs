using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Pedido a central contra WebApiRW (rutas del canon R4). Fase actual: consulta.</summary>
public sealed class PedidoCentralService
{
    private readonly ApiWeb _api;

    public PedidoCentralService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET PedidoCentral/List/{codAlm}?codUsuario=&amp;estado= — pedidos del usuario (0 abiertos, 1 cerrados).</summary>
    public Task<ApiRespuesta<List<PedidoCentral>>> GetPedidosAsync(short codAlm, short codUsuario, byte estado, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoCentral>>($"PedidoCentral/List/{codAlm}?codUsuario={codUsuario}&estado={estado}", ct);

    /// <summary>GET PedidoCentral/{codAlm}/{fecha}/{numPedido}/Lineas — detalle del pedido.</summary>
    public Task<ApiRespuesta<List<PedidoCentralLin>>> GetLineasAsync(short codAlm, DateTime fecha, int numPedido, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoCentralLin>>($"PedidoCentral/{codAlm}/{fecha:yyyy-MM-dd}/{numPedido}/Lineas", ct);

    /// <summary>GET PedidoCentral/List/CentroDistribucion — centros de distribución (para la fase de alta).</summary>
    public Task<ApiRespuesta<List<CentroDistribucion>>> GetCentrosDistAsync(CancellationToken ct = default)
        => _api.GetAsync<List<CentroDistribucion>>("PedidoCentral/List/CentroDistribucion", ct);
}
