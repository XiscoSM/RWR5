namespace R5.Core.Entidades;

/// <summary>Inventario abierto en un almacén (canon R4).</summary>
public sealed class Inventario
{
    public DateTime Fecha { get; set; }
    public short CodAlm { get; set; }
    public long CountProds { get; set; }
}

/// <summary>Línea/conteo de inventario (canon R4).</summary>
public sealed class InventarioLin
{
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public decimal Cant { get; set; }
    public decimal CantTotal { get; set; }
    public short NumTerminal { get; set; }
    public DateTime FechaHoraIntro { get; set; }
}
