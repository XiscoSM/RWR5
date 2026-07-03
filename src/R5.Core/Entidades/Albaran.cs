namespace R5.Core.Entidades;

/// <summary>Albarán de proveedor (cabecera, canon R4).</summary>
public sealed class Albaran
{
    public DateTime Fecha { get; set; }
    public int NumAlbaran { get; set; }
    public Almacen Alm { get; set; } = new();
    public Proveedor Prov { get; set; } = new();
    public int NumPedido { get; set; }
    public bool Reg { get; set; }
    public bool Fact { get; set; }
    public int Propietario { get; set; }
    public long LinkPdf { get; set; }
    public string NumAlbProv { get; set; } = "";
    public string Comentario { get; set; } = "";
    public long Prods { get; set; }
    public decimal CantTotal { get; set; }
    public decimal ImpNetoComercial { get; set; }
    public decimal ImpNetoFact { get; set; }
    public decimal ImpTotalFact { get; set; }
}

/// <summary>Línea de albarán (canon R4).</summary>
public sealed class AlbaranLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public short Cajas { get; set; }
    public decimal Cant { get; set; }
    public bool PrecioAbierto { get; set; }
    public decimal CantAnt { get; set; }
    public double PTarifa { get; set; }
    public double PNC { get; set; }
    public double DtosComerciales { get; set; }
    public string? DtosComercialesTexto { get; set; } = "";
    public Ean Ean { get; set; } = new();
    public byte TipoPrecio { get; set; }
    public byte TipoLinAtipico { get; set; }
    public bool Coment { get; set; }
    public short CajasPedida { get; set; }
    public short UnidsCaja { get; set; }
    public decimal CantPedida { get; set; }
    public decimal CantRecibida { get; set; }
    public decimal CantAlbTotal { get; set; }
    public decimal Stock { get; set; }
    public decimal PncFecha { get; set; }
    public string? Datos { get; set; } = "";
}
