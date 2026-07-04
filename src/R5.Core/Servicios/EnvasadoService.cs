using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Envasado / pesar-etiquetar contra WebApiRW (endpoint aditivo api/Envasado).</summary>
public sealed class EnvasadoService
{
    private readonly ApiWeb _api;

    public EnvasadoService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>
    /// Interpreta el EAN128 de trazabilidad (16 dígitos = 8 producto + 8 lote;
    /// se admite 1 carácter no numérico delante, como en R3).
    /// </summary>
    public static bool TryParseEan128(string codigo, out int prod, out int lote)
    {
        prod = 0;
        lote = 0;
        codigo = codigo.Trim();
        if (codigo.Length == 17 && !char.IsAsciiDigit(codigo[0])) codigo = codigo[1..];
        if (codigo.Length != 16 || !codigo.All(char.IsAsciiDigit)) return false;

        prod = int.Parse(codigo[..8]);
        lote = int.Parse(codigo[8..]);
        return prod > 0 && lote > 0;
    }

    /// <summary>GET Envasado/Lote/{prod}/{lote} — valida el producto padre + lote.</summary>
    public Task<ApiRespuesta<EnvasadoLote>> GetLoteAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetAsync<EnvasadoLote>($"Envasado/Lote/{prod}/{lote}", ct);

    /// <summary>GET Envasado/{prod}/{lote}/Productos?codAlm= — envasables del lote con caducidades.</summary>
    public Task<ApiRespuesta<List<EnvasadoProducto>>> GetProductosAsync(int prod, int lote, short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<EnvasadoProducto>>($"Envasado/{prod}/{lote}/Productos?codAlm={codAlm}", ct);

    /// <summary>GET Report/EtiqEnvasado — PDF de la etiqueta de peso variable (EAN128+CODE128 en servidor).</summary>
    public Task<ApiRespuesta<byte[]>> GetEtiquetaPdfAsync(int prod, int lote, decimal peso, decimal pvp, short almOrigen, string registroSanitario, CancellationToken ct = default)
        => _api.GetBytesAsync(
            $"Report/EtiqEnvasado/{prod}/{lote}?peso={peso.ToString(System.Globalization.CultureInfo.InvariantCulture)}&pvp={pvp.ToString(System.Globalization.CultureInfo.InvariantCulture)}&almOrigen={almOrigen}&registroSanitario={Uri.EscapeDataString(registroSanitario)}", ct);
}
