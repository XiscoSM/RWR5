using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Facturas simplificadas de TPV (módulo 28) contra WebApiRW (api/FacturaTpv,
/// aditivo): tickets de venta → factura con datos fiscales del cliente.</summary>
public sealed class FacturaTpvService
{
    private readonly ApiWeb _api;

    public FacturaTpvService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET FacturaTpv/Cliente?cif= — busca el cliente TPV por CIF.</summary>
    public Task<ApiRespuesta<ClienteTpv>> GetClienteAsync(string cif, string pais = "ES", string unidTramitadora = "", CancellationToken ct = default)
        => _api.GetAsync<ClienteTpv>($"FacturaTpv/Cliente?cif={Uri.EscapeDataString(cif)}&pais={pais}&unidTramitadora={Uri.EscapeDataString(unidTramitadora)}", ct);

    /// <summary>POST FacturaTpv/Cliente — alta/edición del cliente (Cliente 0 = alta).</summary>
    public Task<ApiRespuesta<int>> GuardarClienteAsync(ClienteTpv cliente, short usuario, CancellationToken ct = default)
        => _api.PostAsync<int>($"FacturaTpv/Cliente?usuario={usuario}", cliente, ct);

    /// <summary>POST FacturaTpv/Ticket — añade un ticket a la prefactura; devuelve PreFactura.</summary>
    public Task<ApiRespuesta<int>> AnadirTicketAsync(int preFactura, int cliente, short usuario, string eanTicket, decimal amount = -9999, CancellationToken ct = default)
        => _api.PostAsync<int>($"FacturaTpv/Ticket?preFactura={preFactura}&cliente={cliente}&usuario={usuario}&eanTicket={Uri.EscapeDataString(eanTicket)}&amount={amount}", cuerpo: null, ct);

    /// <summary>DELETE FacturaTpv/Ticket — quita un ticket de la prefactura.</summary>
    public Task<ApiRespuesta> QuitarTicketAsync(int preFactura, short storeNo, short termNo, int transNo, CancellationToken ct = default)
        => _api.DeleteAsync($"FacturaTpv/Ticket?preFactura={preFactura}&storeNo={storeNo}&termNo={termNo}&transNo={transNo}", ct);

    /// <summary>GET FacturaTpv/Ticket?eanTicket= — detalle de un ticket (filas dinámicas).</summary>
    public Task<ApiRespuesta<List<Dictionary<string, string?>>>> GetTicketAsync(string eanTicket, CancellationToken ct = default)
        => _api.GetAsync<List<Dictionary<string, string?>>>($"FacturaTpv/Ticket?eanTicket={Uri.EscapeDataString(eanTicket)}", ct);

    /// <summary>GET FacturaTpv/Relacion/{cliente} — prefacturas y facturas del cliente.</summary>
    public Task<ApiRespuesta<List<Dictionary<string, string?>>>> GetRelacionAsync(int cliente, short usuario, CancellationToken ct = default)
        => _api.GetAsync<List<Dictionary<string, string?>>>($"FacturaTpv/Relacion/{cliente}?usuario={usuario}", ct);

    /// <summary>POST FacturaTpv/Facturar/{preFactura} — registra la prefactura como factura.</summary>
    public Task<ApiRespuesta<int>> FacturarAsync(int preFactura, short usuario, string observaciones = "", CancellationToken ct = default)
        => _api.PostAsync<int>($"FacturaTpv/Facturar/{preFactura}?usuario={usuario}&observaciones={Uri.EscapeDataString(observaciones)}", cuerpo: null, ct);

    /// <summary>DELETE FacturaTpv/{factura} — borra la factura (cotejando cliente/fecha/importe).</summary>
    public Task<ApiRespuesta> BorrarFacturaAsync(string factura, int cliente, DateTime fecha, decimal importeTotal, string usuario, CancellationToken ct = default)
        => _api.DeleteAsync($"FacturaTpv/{Uri.EscapeDataString(factura)}?cliente={cliente}&fecha={fecha:yyyy-MM-dd}&importeTotal={importeTotal}&usuario={Uri.EscapeDataString(usuario)}", ct);

    /// <summary>POST FacturaTpv/{factura}/ColaMail — encola el envío de la factura por mail.</summary>
    public Task<ApiRespuesta<int>> EnviarMailAsync(string factura, short usuario, string toMail, CancellationToken ct = default)
        => _api.PostAsync<int>($"FacturaTpv/{Uri.EscapeDataString(factura)}/ColaMail?usuario={usuario}&toMail={Uri.EscapeDataString(toMail)}", cuerpo: null, ct);

    /// <summary>GET Report/FacturaTpv/{factura} — PDF de la factura (endpoint ya existente).</summary>
    public Task<ApiRespuesta<byte[]>> GetPdfAsync(string factura, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/FacturaTpv/{Uri.EscapeDataString(factura)}", ct);
}
