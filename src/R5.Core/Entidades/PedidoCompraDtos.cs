namespace R5.Core.Entidades;

/// <summary>Alta de cabecera de pedido a proveedor (DTO del canon R4, POST PedidoCompra/{fecha}/{gama}).</summary>
public sealed class PedidoCompraInsertDTO
{
    public int NumPedido { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime FechaPrevEnvio { get; set; }
    public int CodProv { get; set; }
    public int CodGama { get; set; }
    public short CodUsuario { get; set; }
    public short CodAlm { get; set; }
}

/// <summary>Alta de línea de pedido a proveedor (DTO del canon R4, POST PedidoCompra/{num}/Linea).</summary>
public sealed class PedidoCompraLinInsertDTO
{
    public int NumPedido { get; set; }
    public DateTime Fecha { get; set; }
    public int CodProv { get; set; }
    public int CodGama { get; set; }
    public int CodProd { get; set; }
    public long CodEan { get; set; }
    public string CodProdProv { get; set; } = "";
    public short CodUsuario { get; set; }
    public decimal Cant { get; set; }
    public short Cajas { get; set; }
    public short CodAlm { get; set; }
}

/// <summary>Solicitud de etiqueta Madisa (DTO del canon R4, POST Madisa/Etiqueta/{alm}/Ean).
/// Option: 0 solo consulta, 1 insertar etiqueta PDA, 2 consulta+etiqueta PDA,
/// 3 actualizar PriceItem (etiqueta del día), 4 consulta+PriceItem.</summary>
public sealed class EanUserTermDTO
{
    public long CodEan { get; set; }
    public short CodAlm { get; set; }
    public short CodUsuario { get; set; }
    public short NumTerminal { get; set; }
    public byte Option { get; set; }
}
