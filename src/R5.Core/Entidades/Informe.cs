namespace R5.Core.Entidades;

/// <summary>
/// Tipo de informe manual (canon R4). Cada byte de visibilidad:
/// 0 no visible, 1 visible, 2 obligatorio. Config completa para la fase de alta.
/// </summary>
public sealed class TipoInforme
{
    public byte Tipo { get; set; }
    public string? DescTipo { get; set; }
    public byte Cajas { get; set; }
    public byte Cant { get; set; }
    public byte AlmDest { get; set; }
    public byte PrecioA { get; set; }
    public byte PrecioB { get; set; }
    public byte PrecioC { get; set; }
    public byte PrecioA_Tipo { get; set; }
    public byte PrecioB_Tipo { get; set; }
    public byte PrecioC_Tipo { get; set; }
    public string? PrecioA_Desc { get; set; }
    public string? PrecioB_Desc { get; set; }
    public string? PrecioC_Desc { get; set; }
    public byte ValorA { get; set; }
    public byte ValorA_Tipo { get; set; }
    public byte ValorB { get; set; }
    public byte ValorB_Tipo { get; set; }
    public byte ValorC { get; set; }
    public byte ValorC_Tipo { get; set; }
    public string? ValorA_Desc { get; set; }
    public string? ValorB_Desc { get; set; }
    public string? ValorC_Desc { get; set; }
    public bool RellenarCampos { get; set; }
    public bool NoInsert { get; set; }
}

/// <summary>Informe manual (cabecera, canon R4).</summary>
public sealed class Informe
{
    public DateTime Fecha { get; set; }
    public int NumInforme { get; set; }
    public TipoInforme? TipoInforme { get; set; }
    public Almacen? AlmOrigen { get; set; }
    public Almacen? AlmDest { get; set; }
    public bool Estado { get; set; }
    public long Prods { get; set; }
    public short CodUsuario { get; set; }
}

/// <summary>Línea de informe (canon R4). Los campos opcionales dependen del TipoInforme.</summary>
public sealed class InformeLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public short? Cajas { get; set; }
    public decimal? Cant { get; set; }
    public decimal? PrecioA { get; set; }
    public decimal? PrecioB { get; set; }
    public decimal? PrecioC { get; set; }
    public string? ValorA { get; set; }
    public string? ValorB { get; set; }
    public string? ValorC { get; set; }
}
