using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// Pedido de Cliente a Central (módulo 17 de R3) contra WebApiRW (api/PedidoCliente,
/// aditivo). El mostrador toma nota y envía; el central prepara (bandeja por línea).
/// El estado habilita cada acción — nunca el tipo de almacén.
/// </summary>
public sealed class PedidoClienteService
{
    private readonly ApiWeb _api;

    public PedidoClienteService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET PedidoCliente/List/{alm} — listado paginado del mostrador con filtros.</summary>
    public Task<ApiRespuesta<List<PedidoClienteCab>>> GetListaAsync(short codAlm, int estadoDoc = -1,
        string cliente = "", long telefono = 0, int pagina = 1, int tamPagina = 200, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoClienteCab>>(
            $"PedidoCliente/List/{codAlm}?estadoDoc={estadoDoc}&cliente={Uri.EscapeDataString(cliente)}&telefono={telefono}&pagina={pagina}&tamPagina={tamPagina}", ct);

    /// <summary>GET PedidoCliente/{pedido} — cabecera con estado real.</summary>
    public Task<ApiRespuesta<PedidoClienteCab>> GetCabAsync(int pedido, CancellationToken ct = default)
        => _api.GetAsync<PedidoClienteCab>($"PedidoCliente/{pedido}", ct);

    /// <summary>GET PedidoCliente/{pedido}/Lineas.</summary>
    public Task<ApiRespuesta<List<PedidoClienteLin>>> GetLineasAsync(int pedido, CancellationToken ct = default)
        => _api.GetAsync<List<PedidoClienteLin>>($"PedidoCliente/{pedido}/Lineas", ct);

    /// <summary>GET PedidoCliente/Bandeja/{alm} — bandeja del preparador a nivel de línea.
    /// ambito 0 = A preparar (soy el central), 1 = Mis pedidos (tomé yo nota).</summary>
    public Task<ApiRespuesta<List<PedidoClienteLin>>> GetBandejaAsync(short codAlm, int ambito,
        DateTime? fDesde = null, DateTime? fHasta = null, int estadoLin = -1, CancellationToken ct = default)
    {
        string fechas = (fDesde is null ? "" : $"&fDesde={fDesde:yyyy-MM-dd}")
                      + (fHasta is null ? "" : $"&fHasta={fHasta:yyyy-MM-dd}");
        return _api.GetAsync<List<PedidoClienteLin>>(
            $"PedidoCliente/Bandeja/{codAlm}?ambito={ambito}&estadoLin={estadoLin}{fechas}", ct);
    }

    /// <summary>GET PedidoCliente/EanAlm/{ean}/{alm} — producto para añadir línea escaneando.</summary>
    public Task<ApiRespuesta<PedidoClienteLin>> GetProductoPorEanAsync(long ean, short codAlm, CancellationToken ct = default)
        => _api.GetAsync<PedidoClienteLin>($"PedidoCliente/EanAlm/{ean}/{codAlm}", ct);

    /// <summary>POST PedidoCliente/Cab — crea el pedido y devuelve su número.</summary>
    public Task<ApiRespuesta<int>> PostCabAsync(PedidoClienteCabDTO cab, CancellationToken ct = default)
        => _api.PostAsync<int>("PedidoCliente/Cab", cab, ct);

    /// <summary>PUT PedidoCliente/Cab/{pedido} — edita los datos del cliente/entrega.</summary>
    public Task<ApiRespuesta<int>> PutCabAsync(int pedido, PedidoClienteCabDTO cab, CancellationToken ct = default)
        => _api.PutAsync<int>($"PedidoCliente/Cab/{pedido}", cab, ct);

    public Task<ApiRespuesta<int>> EnviarAsync(int pedido, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCliente/{pedido}/Enviar", cuerpo: null, ct);

    public Task<ApiRespuesta<int>> CancelarAsync(int pedido, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCliente/{pedido}/Cancelar", cuerpo: null, ct);

    /// <summary>Pasa a Preparación todas las líneas pendientes (acción masiva del central).</summary>
    public Task<ApiRespuesta<int>> PrepararTodoAsync(int pedido, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCliente/{pedido}/PrepararTodo", cuerpo: null, ct);

    /// <summary>POST PedidoCliente/{pedido}/Linea — añade línea; devuelve su número.</summary>
    public Task<ApiRespuesta<int>> PostLineaAsync(int pedido, PedidoClienteLinDTO lin, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCliente/{pedido}/Linea", lin, ct);

    public Task<ApiRespuesta<int>> PutLineaAsync(int pedido, int linea, PedidoClienteLinDTO lin, CancellationToken ct = default)
        => _api.PutAsync<int>($"PedidoCliente/{pedido}/Linea/{linea}", lin, ct);

    public Task<ApiRespuesta> DeleteLineaAsync(int pedido, int linea, CancellationToken ct = default)
        => _api.DeleteAsync($"PedidoCliente/{pedido}/Linea/{linea}", ct);

    /// <summary>Único punto de cambio de estado de línea (Preparar/Entregar/No disp./Reabrir):
    /// el proc valida la transición y recalcula el estado del documento.</summary>
    public Task<ApiRespuesta<int>> CambiarEstadoLineaAsync(int pedido, int linea, byte estado, CancellationToken ct = default)
        => _api.PostAsync<int>($"PedidoCliente/{pedido}/Linea/{linea}/Estado/{estado}", cuerpo: null, ct);

    // ---- Selector Programa → Tecla Madisa → Producto (catálogo sin escanear) ----

    public Task<ApiRespuesta<List<ProgramaMadisa>>> GetMadisaProgramasAsync(CancellationToken ct = default)
        => _api.GetAsync<List<ProgramaMadisa>>("PedidoCliente/Madisa/Programas", ct);

    public Task<ApiRespuesta<List<TeclaMadisa>>> GetMadisaTeclasAsync(short prog, CancellationToken ct = default)
        => _api.GetAsync<List<TeclaMadisa>>($"PedidoCliente/Madisa/Programas/{prog}/Teclas", ct);

    public Task<ApiRespuesta<List<ProductoMadisa>>> GetMadisaProductosAsync(short tecla, short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<ProductoMadisa>>($"PedidoCliente/Madisa/Teclas/{tecla}/Productos/{codAlm}", ct);
}
