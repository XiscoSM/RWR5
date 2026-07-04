namespace R5.Core.Entidades;

/// <summary>Producto padre + lote resuelto del EAN128 de trazabilidad (canon R4, api/Envasado).</summary>
public sealed class EnvasadoLote
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public int Lote { get; set; }
    public byte TipoLote { get; set; }
    public string DescTipoLote { get; set; } = "";
}

/// <summary>Producto envasable de un lote, con caducidades (canon R4).</summary>
public sealed class EnvasadoProducto
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public decimal Pvp { get; set; }
    public bool Caducado { get; set; }
    public bool LoteCaducado { get; set; }
    public DateTime? FechaCad { get; set; }
    public DateTime? FechaCadLote { get; set; }
}
