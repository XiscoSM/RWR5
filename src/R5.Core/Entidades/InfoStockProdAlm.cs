namespace R5.Core.Entidades;

/// <summary>Stock de un producto en un almacén/tienda (canon R4, GET api/ListStockProdAlm/{codProd}).</summary>
public sealed class InfoStockProdAlm
{
    public short CodAlm { get; set; }
    public string DescAlm { get; set; } = "";
    public decimal Stock { get; set; }
}
