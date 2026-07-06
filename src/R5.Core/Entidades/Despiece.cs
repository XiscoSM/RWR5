namespace R5.Core.Entidades;

/// <summary>Producto padre + lote resuelto del EAN128 (validación del despiece).</summary>
public sealed class DespieceLote
{
    public int Prod { get; set; }
    public int Lote { get; set; }
    public string DescProd { get; set; } = "";
    public decimal PrecioCoste { get; set; }
}

/// <summary>
/// Línea de despiece: componente de un grupo. Las filas con Prod=0 son el título
/// del grupo (así las devuelve el proc y las pinta R3).
/// </summary>
public sealed class DespieceLin
{
    public int Despiece { get; set; }
    public int Prod { get; set; }
    public string DescProdCompleta { get; set; } = "";
    public short NumEtiqPrin { get; set; }
    public short NumEtiqPrint { get; set; }
}

/// <summary>Alta de lote interno para una línea de traspaso recibida (módulo 31).</summary>
public sealed class LoteTraspasoInsertDTO
{
    public int Prod { get; set; }
    public DateTime FechaTrasp { get; set; }
    public int Traspaso { get; set; }
    public int LineaTrasp { get; set; }
    public int NumLoteProv { get; set; }
}
