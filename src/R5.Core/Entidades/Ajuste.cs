namespace R5.Core.Entidades;

/// <summary>Programa de gestión (destino de ajustes, canon R4).</summary>
public sealed class Programa
{
    public short Prog { get; set; } = -1;
    public string? DescProg { get; set; } = "";
}

/// <summary>Tipo de ajuste de stock (canon R4).</summary>
public sealed class TipoAjuste
{
    public byte Tipo { get; set; }
    public string? DescTipo { get; set; }
    public string Cuenta { get; set; } = "";
    public bool PermiteCambioAlmDestino { get; set; }
    public bool PermiteCambioProgDestino { get; set; }
}

/// <summary>Ajuste de stock (cabecera, canon R4).</summary>
public sealed class Ajuste
{
    public int NumAjuste { get; set; }
    public TipoAjuste TipoAjuste { get; set; } = new();
    public Programa? ProgDestinoCab { get; set; }
    public Almacen? AlmOrigen { get; set; }
    public Almacen? AlmDest { get; set; }
    public bool Reg { get; set; }
    public long Prods { get; set; }
    public decimal ImporteCoste { get; set; }
    public DateTime Fecha { get; set; }
}

/// <summary>Línea de ajuste (canon R4).</summary>
public sealed class AjusteLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public decimal Cant { get; set; }
    public decimal PrecioCoste { get; set; }
    public int Lote { get; set; }
    public short ProgOrigen { get; set; }
    public short ProgDest { get; set; }
    public Ean Ean { get; set; } = new();
    public decimal TotalCantEnAjuste { get; set; }
}
