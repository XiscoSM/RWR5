namespace R5.Core.Entidades;

/// <summary>
/// Producto. Propiedades = entidad de WebApiRW (canon R4); las fechas de última
/// compra son nullables como en la API (en RWR4-cliente no lo eran y daba 01/01/0001).
/// </summary>
public sealed class Producto
{
    public int CodProd { get; set; }
    public string? DescProd { get; set; } = "";
    public bool DecimalesEnCant { get; set; }
    public decimal UltPrecioCoste { get; set; }
    public decimal? Pvp { get; set; }
    public decimal? PvpSinOferta { get; set; }
    public DateTime? OfertaHasta { get; set; }
    public bool ControlaLotes { get; set; }
    public short UnidsCaja { get; set; } = 1;
    public short DiasCaducidad { get; set; }
    public DateTime? FechaUltCompra { get; set; }
    public DateTime? FechaUltCompraAlm { get; set; }
    public DateTime? FechaCuracion { get; set; }
    public DateTime? FechaCaducidad { get; set; }
    public int? Lote { get; set; }
    public short CajasPorPalet { get; set; } = 1;
    public byte CajasPorCapa { get; set; } = 1;
    public bool FueraColeccion { get; set; }
    public bool BloqueoCompra { get; set; }
    public decimal? Stock { get; set; }
    public int? StockMin { get; set; }
    public int? StockMax { get; set; }
    public long? CodEan { get; set; }
    public string Ubicacion { get; set; } = "0";
    public bool BloqueoPedAlm { get; set; }
    public decimal Iva { get; set; }
    public string? ProdProveedor { get; set; }
    public int Prov { get; set; }
    public int ProvAlm { get; set; }
    public string? ProdProvAlm { get; set; }

    /// <summary>Unidad de venta: los productos a peso llevan decimales en cantidad.</summary>
    public string TipoUnidad => DecimalesEnCant ? "Kilos" : "Unids";
}
