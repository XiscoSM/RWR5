namespace R5.Core.Entidades;

/// <summary>Gama de compra de un proveedor (canon R4).</summary>
public sealed class Gama
{
    public int CodGama { get; set; }
    public string DescGama { get; set; } = "";
    public bool ControlCant { get; set; }
    public bool ControlImporte { get; set; }
    public bool NoIntroEanCompra { get; set; }
}

/// <summary>Proveedor (canon R4).</summary>
public sealed class Proveedor
{
    public int CodProv { get; set; }
    public string DescProv { get; set; } = "";
    public Gama Gama { get; set; } = new();
}
