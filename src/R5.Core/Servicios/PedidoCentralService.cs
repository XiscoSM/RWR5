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

    /// <summary>GET PedidoCentral/{alm}/Prod?codEan= — producto para pedir, por código de barras.</summary>
    public Task<ApiRespuesta<PedidoCentralLin>> GetLineaPorEanAsync(short codAlm, long codEan, CancellationToken ct = default)
        => _api.GetAsync<PedidoCentralLin>($"PedidoCentral/{codAlm}/Prod?codEan={codEan}", ct);

    /// <summary>GET PedidoCentral/{alm}/Prod/{codProd} — producto para pedir, por código.</summary>
    public Task<ApiRespuesta<PedidoCentralLin>> GetLineaPorProdAsync(short codAlm, int codProd, CancellationToken ct = default)
        => _api.GetAsync<PedidoCentralLin>($"PedidoCentral/{codAlm}/Prod/{codProd}", ct);

    /// <summary>GET .../LineaDto — pedido abierto del centro, central que sirve, stock y cant. ya pedida.</summary>
    public Task<ApiRespuesta<PedidoCentralLineaSelectDTO>> GetLineaDtoAsync(short codAlm, int codProd, short codUsuario, byte centroDist, decimal cant, CancellationToken ct = default)
        => _api.GetAsync<PedidoCentralLineaSelectDTO>(
            $"PedidoCentral/{codAlm}/Prod/{codProd}/LineaDto?codUsuario={codUsuario}&centroDist={centroDist}&cant={cant.ToString(System.Globalization.CultureInfo.InvariantCulture)}", ct);

    /// <summary>POST PedidoCentral/{alm}/{prod} — añade la línea (crea el pedido si no hay uno abierto).</summary>
    public Task<ApiRespuesta<PedidoCentral>> PostLineaAsync(PedidoCentralLinInsertDTO linea, CancellationToken ct = default)
        => _api.PostAsync<PedidoCentral>($"PedidoCentral/{linea.CodAlm}/{linea.CodProd}", linea, ct);

    /// <summary>POST .../CambiarEstado — registra (cierra) el pedido para que central lo prepare.</summary>
    public Task<ApiRespuesta> RegistrarAsync(short codAlm, DateTime fecha, int numPedido, CancellationToken ct = default)
        => _api.PostAsync($"PedidoCentral/{codAlm}/{fecha:yyyy-MM-dd}/{numPedido}/CambiarEstado", cuerpo: null, ct);
}
