namespace R5.Core.Entidades;

/// <summary>Pedido a proveedor (cabecera, canon R4).</summary>
public sealed class PedidoCompra
{
    public DateTime Fecha { get; set; }
    public int NumPedido { get; set; }
    public DateTime FechaPrevEnvio { get; set; }
    public Almacen Alm { get; set; } = new();
    public Proveedor Prov { get; set; } = new();
    public bool Reg { get; set; }
    public string Comentario { get; set; } = "";
    public int Prods { get; set; }
    public string Mail { get; set; } = "";
}

/// <summary>Línea de pedido a proveedor (canon R4).</summary>
public sealed class PedidoCompraLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public decimal PTarifa { get; set; }
    public decimal AtipicosPedido { get; set; }
    public decimal PNC { get; set; }
    public decimal DtosComerciales { get; set; }
    public string DtosComercialesTexto { get; set; } = "";
    public decimal DtoPorc { get; set; }
    public decimal DtoCant { get; set; }
    public decimal DtoDivisa { get; set; }
    public int Lote { get; set; }
    public byte TipoPrecio { get; set; }
    public byte TipoLinAtipico { get; set; }
    public bool Coment { get; set; }
    public bool PrecioAbierto { get; set; }
    public string Comentario { get; set; } = "";
    public short Cajas { get; set; }
    public decimal Cant { get; set; }
    public int NumPedido { get; set; }
    public decimal Stock { get; set; }
    public decimal CantRecibida { get; set; }
}
