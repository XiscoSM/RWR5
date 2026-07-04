namespace R5.Core.Entidades;

/// <summary>Alta de línea de pedido a central (DTO del canon R4, POST PedidoCentral/{alm}/{prod}).</summary>
public sealed class PedidoCentralLinInsertDTO
{
    public int CodProd { get; set; }
    public int CodUsuario { get; set; }
    public short CodAlm { get; set; }
    public short CodAlmCentral { get; set; }
    public decimal CantPedida { get; set; }
    public DateTime Fecha { get; set; }
    public int NumPedido { get; set; }
}

/// <summary>
/// Contexto de línea que devuelve la API antes del alta (canon R4, GET .../LineaDto):
/// pedido abierto del centro (si existe), central que sirve y stock/ya pedida.
/// CodAlmCentral = 0 significa que ese centro de distribución no sirve el producto.
/// </summary>
public sealed class PedidoCentralLineaSelectDTO
{
    public decimal StockAlmCentral { get; set; }
    public short CodAlmCentral { get; set; }
    public byte RecibirPedCentral { get; set; }
    public DateTime Fecha { get; set; }
    public int NumPedido { get; set; }
    public byte Estado { get; set; }
    public decimal CantYaPedida { get; set; }
}
