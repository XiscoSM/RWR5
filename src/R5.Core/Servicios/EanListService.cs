using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// Cola de etiquetas EanList (X_EtiquetasR2) contra WebApiRW (endpoints aditivos
/// api/EanList + api/Report/EanList*). Port del módulo 12 de R3 (Delphi Fex Etiqueta).
/// </summary>
public sealed class EanListService
{
    private readonly ApiWeb _api;

    public EanListService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET EanList/Pendientes/{alm}?etiqPda=&amp;equipo= — cola de pendientes.
    /// etiqPda: true = pedidas desde terminal, false = cambios de PVP del día.</summary>
    public Task<ApiRespuesta<List<EanListPendiente>>> GetPendientesAsync(short codAlm, bool etiqPda, string equipo = "", CancellationToken ct = default)
        => _api.GetAsync<List<EanListPendiente>>($"EanList/Pendientes/{codAlm}?etiqPda={etiqPda}&equipo={Uri.EscapeDataString(equipo)}", ct);

    /// <summary>POST EanList/Carga/{alm}/{modo} — carga la cola del día
    /// (1 = cambios de PVP del día, 0 = reimpresión de cambios, 2 = reimprimir).</summary>
    public Task<ApiRespuesta> CargarAsync(short codAlm, byte modo, CancellationToken ct = default)
        => _api.PostAsync($"EanList/Carga/{codAlm}/{modo}", cuerpo: null, ct);

    /// <summary>POST EanList/Marcar/{alm} — marca como impresas (borra) las etiquetas sacadas.</summary>
    public Task<ApiRespuesta> MarcarAsync(short codAlm, bool etiqPda, string equipo = "", CancellationToken ct = default)
        => _api.PostAsync($"EanList/Marcar/{codAlm}?etiqPda={etiqPda}&equipo={Uri.EscapeDataString(equipo)}", cuerpo: null, ct);

    /// <summary>GET EanList/MetoAlb/{alm}/{fecha}?albaran= — previsualización de etiquetas de albarán.</summary>
    public Task<ApiRespuesta<List<EanListMetoAlbLinea>>> GetMetoAlbAsync(short codAlm, DateTime fecha, string albaran, CancellationToken ct = default)
        => _api.GetAsync<List<EanListMetoAlbLinea>>($"EanList/MetoAlb/{codAlm}/{fecha:yyyy-MM-dd}?albaran={Uri.EscapeDataString(albaran)}", ct);

    // ---------- PDFs (api/Report) ----------

    /// <summary>PDF de la cola de un formato (tipoPrint 0-5). Fruta admite imprimirPrecio.</summary>
    public Task<ApiRespuesta<byte[]>> GetColaPdfAsync(short codAlm, byte tipoPrint, bool etiqPda, string equipo = "", bool imprimirPrecio = true, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EanListCola/{codAlm}/{tipoPrint}?etiqPda={etiqPda}&equipo={Uri.EscapeDataString(equipo)}&imprimirPrecio={imprimirPrecio}", ct);

    /// <summary>PDF de etiquetas de estantería (Meto) de un albarán.</summary>
    public Task<ApiRespuesta<byte[]>> GetMetoAlbPdfAsync(short codAlm, DateTime fecha, string albaran, bool conPvp, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EanListMetoAlb/{codAlm}/{fecha:yyyy-MM-dd}?albaran={Uri.EscapeDataString(albaran)}&conPvp={conPvp}", ct);

    /// <summary>PDF de etiqueta manual (EAN + texto + PVP opcional, N copias).</summary>
    public Task<ApiRespuesta<byte[]>> GetMetoManualPdfAsync(string ean, string texto, decimal pvp, short copias, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EanListMeto?ean={Uri.EscapeDataString(ean)}&texto={Uri.EscapeDataString(texto)}&pvp={pvp}&copias={copias}", ct);

    /// <summary>PDF de serie numérica consecutiva de etiquetas (EAN13 con dígito de control).</summary>
    public Task<ApiRespuesta<byte[]>> GetSeriePdfAsync(long inicio, short cantidad, string texto, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EanListSerie?inicio={inicio}&cantidad={cantidad}&texto={Uri.EscapeDataString(texto)}", ct);

    /// <summary>PDF de etiquetas de bulto de traspaso (EAN '1'+origen+destino+bulto+DC).</summary>
    public Task<ApiRespuesta<byte[]>> GetBultosPdfAsync(short origen, short destino, short bultoIni, short bultoFin, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EanListTraspasos/{origen}/{destino}?bultoIni={bultoIni}&bultoFin={bultoFin}", ct);
}
