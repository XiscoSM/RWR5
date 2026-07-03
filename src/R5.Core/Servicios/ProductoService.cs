using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Consulta de productos contra WebApiRW (rutas del canon R4).</summary>
public sealed class ProductoService
{
    private readonly ApiWeb _api;

    // Mismo repositorio de imágenes que usa RWR4; si algún día cambia, solo se toca aquí.
    private const string UrlBaseImagenes = "https://tuhiper.com/images/Prod/4k";

    public ProductoService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Productos?codEan= — resuelve un código de barras a producto.</summary>
    public Task<ApiRespuesta<Producto>> GetPorEanAsync(long codEan, CancellationToken ct = default)
        => _api.GetAsync<Producto>($"Productos?codEan={codEan}", ct);

    /// <summary>GET Productos/{codProd}/Info — ficha completa (pvp, stock del almacén, últimas compras).</summary>
    public Task<ApiRespuesta<Producto>> GetInfoAsync(int codProd, short codAlm, long codEan = 0, CancellationToken ct = default)
        => _api.GetAsync<Producto>($"Productos/{codProd}/Info?codAlm={codAlm}&codEan={codEan}", ct);

    /// <summary>GET Eans?codProd= — códigos de barras del producto (unidad y caja).</summary>
    public Task<ApiRespuesta<List<Ean>>> GetEansAsync(int codProd, CancellationToken ct = default)
        => _api.GetAsync<List<Ean>>($"Eans?codProd={codProd}", ct);

    /// <summary>GET ListStockProdAlm/{codProd} — stock del producto en todos los almacenes/tiendas.</summary>
    public Task<ApiRespuesta<List<InfoStockProdAlm>>> GetStockAlmacenesAsync(int codProd, CancellationToken ct = default)
        => _api.GetAsync<List<InfoStockProdAlm>>($"ListStockProdAlm/{codProd}", ct);

    /// <summary>URL de la foto del producto (subcarpeta = primer dígito del código, como en RWR4).</summary>
    public static string UrlImagen(int codProd)
    {
        string codigo = codProd.ToString();
        return $"{UrlBaseImagenes}/{codigo[0]}/{codigo}.JPG";
    }
}
