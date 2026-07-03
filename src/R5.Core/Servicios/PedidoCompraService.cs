using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Pedido a proveedor contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class PedidoCompraService
{
    private readonly ApiWeb _api;

    public PedidoCompraService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET PedidoCompra/List/{codAlm}?codUsuario=&amp;reg= — pedidos del usuario.</summary>
    public Task<ApiRespuesta<List<PedidoCompra>>> GetPedidosAsync(short codAlm, short codUsuario, bool reg, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoCompra>>($"PedidoCompra/List/{codAlm}?codUsuario={codUsuario}&reg={reg}", ct);

    /// <summary>GET PedidoCompra/{num}/Lineas — detalle del pedido.</summary>
    public Task<ApiRespuesta<List<PedidoCompraLin>>> GetLineasAsync(int numPedido, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoCompraLin>>($"PedidoCompra/{numPedido}/Lineas?top={top}", ct);
}
