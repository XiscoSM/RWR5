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
    public Task<ApiRespuesta<List<Albaran>>> GetAlbaranesAsync(short codAlm, short codUsuario, bool registrado, short top = 300, CancellationToken ct = default)
        => _api.GetAsync<List<Albaran>>($"Albaran/List/{codAlm}?codUsuario={codUsuario}&registrado={registrado}&top={top}", ct);

    /// <summary>GET Albaran/{num}/{fecha} — cabecera del albarán (estado real Reg, proveedor, totales).</summary>
    public Task<ApiRespuesta<Albaran>> GetCabeceraAsync(int numAlbaran, DateTime fecha, CancellationToken ct = default)
        => _api.GetAsync<Albaran>($"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}", ct);

    /// <summary>GET Albaran/{num}/{fecha}/Lineas — detalle del albarán.</summary>
    public Task<ApiRespuesta<List<AlbaranLin>>> GetLineasAsync(int numAlbaran, DateTime fecha, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<AlbaranLin>>($"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}/Lineas?top={top}", ct);

    /// <summary>GET Albaran/{num}/{fecha}/Ean/{ean} — línea a recepcionar con precios (num 0 = albarán nuevo).</summary>
    public Task<ApiRespuesta<AlbaranLin>> GetLineaPorEanAsync(int numAlbaran, DateTime fecha, long codEan, short codAlm, int codProv, int gama, int numPedido = 0, CancellationToken ct = default)
        => _api.GetAsync<AlbaranLin>(
            $"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}/Ean/{codEan}?codAlm={codAlm}&codProv={codProv}&gama={gama}&numPedido={numPedido}", ct);

    /// <summary>GET Albaran/{num}/{fecha}/Prod/{codProd} — línea a recepcionar, por código de producto.</summary>
    public Task<ApiRespuesta<AlbaranLin>> GetLineaPorProdAsync(int numAlbaran, DateTime fecha, int codProd, short codAlm, int codProv, int gama, int numPedido = 0, CancellationToken ct = default)
        => _api.GetAsync<AlbaranLin>(
            $"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}/Prod/{codProd}?codAlm={codAlm}&codProv={codProv}&gama={gama}&numPedido={numPedido}", ct);

    /// <summary>POST Albaran/{alm}/{fecha}/{prod} — añade línea (NumAlbaran 0 crea el albarán; devuelve su número).</summary>
    public Task<ApiRespuesta<Documento>> PostLineaAsync(AlbLinInsertDTO linea, CancellationToken ct = default)
        => _api.PostAsync<Documento>($"Albaran/{linea.CodAlm}/{linea.Fecha:yyyy-MM-dd}/{linea.CodProd}", linea, ct);

    /// <summary>POST Albaran/{num}/{fecha}/Registrar/{codUsuario}/Link/{link} — registra la recepción.</summary>
    public Task<ApiRespuesta> RegistrarAsync(int numAlbaran, DateTime fecha, short codUsuario, long link = 0, CancellationToken ct = default)
        => _api.PostAsync($"Albaran/{numAlbaran}/{fecha:yyyy-MM-dd}/Registrar/{codUsuario}/Link/{link}", cuerpo: null, ct);
}
