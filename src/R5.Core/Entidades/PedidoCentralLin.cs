namespace R5.Core.Entidades;

/// <summary>Línea de pedido a central (canon R4): producto + cantidades y stocks de referencia.</summary>
public sealed class PedidoCentralLin
{
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public decimal StockAlmCentral { get; set; }
    public decimal StockAlm { get; set; }
    public decimal CantPedida { get; set; }
    public short CajasPedidas { get; set; }
}
