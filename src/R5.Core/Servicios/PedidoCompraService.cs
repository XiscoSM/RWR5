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

    /// <summary>POST PedidoCompra/{fecha}/{gama} — crea la cabecera y devuelve el número de pedido.</summary>
    public Task<ApiRespuesta<int>> PostCabeceraAsync(PedidoCompraInsertDTO cabecera, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCompra/{cabecera.Fecha:yyyy-MM-dd}/{cabecera.CodGama}", cabecera, ct);

    /// <summary>GET PedidoCompra/{num}/Producto/{codProd}?…&amp;codEan= — producto de la gama con precios/stock.</summary>
    public Task<ApiRespuesta<PedidoCompraLin>> GetProductoAsync(int numPedido, DateTime fecha, int codProv, int codGama, int codProd, long codEan, short codAlm, CancellationToken ct = default)
        => _api.GetAsync<PedidoCompraLin>(
            $"PedidoCompra/{numPedido}/Producto/{codProd}?fecha={fecha:yyyy-MM-dd}&codProv={codProv}&codGama={codGama}&codEan={codEan}&codAlm={codAlm}", ct);

    /// <summary>POST PedidoCompra/{num}/Linea — añade la línea al pedido.</summary>
    public Task<ApiRespuesta<PedidoCompraLin>> PostLineaAsync(PedidoCompraLinInsertDTO linea, CancellationToken ct = default)
        => _api.PostAsync<PedidoCompraLin>($"PedidoCompra/{linea.NumPedido}/Linea", linea, ct);

    /// <summary>POST PedidoCompra/{num}/Registrar/{codUsuario}/{fechaPrevEnvio} — registra el pedido.</summary>
    public Task<ApiRespuesta> RegistrarAsync(int numPedido, short codUsuario, DateTime fechaPrevEnvio, CancellationToken ct = default)
        => _api.PostAsync($"PedidoCompra/{numPedido}/Registrar/{codUsuario}/{fechaPrevEnvio:yyyy-MM-dd}", cuerpo: null, ct);

    /// <summary>POST PedidoCompra/{num}/ColaMail?codUsuario= — encola el envío por mail al proveedor.</summary>
    public Task<ApiRespuesta<string>> EnviarMailAsync(int numPedido, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync<string>($"PedidoCompra/{numPedido}/ColaMail?codUsuario={codUsuario}", cuerpo: null, ct);
}
