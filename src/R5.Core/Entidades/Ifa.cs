namespace R5.Core.Entidades;

/// <summary>Línea de pedido en la central de compras IFA (consulta, módulo 35).</summary>
public sealed class IfaPedidoLin
{
    public string Proveedor { get; set; } = "";
    public string Pedido { get; set; } = "";
    public string Status { get; set; } = "";
    public string FechaPedido { get; set; } = "";
    public string FechaPrevista { get; set; } = "";
    public string Ean { get; set; } = "";
    public double Cantidad { get; set; }
    public string PuntoDestino { get; set; } = "";
}

/// <summary>Línea de recepción en la central de compras IFA (consulta, módulo 35).</summary>
public sealed class IfaRecepcionLin
{
    public string Proveedor { get; set; } = "";
    public string Pedido { get; set; } = "";
    public string FechaRecepcion { get; set; } = "";
    public string Ean { get; set; } = "";
    public double CantidadRecibida { get; set; }
    public string PuntoDestino { get; set; } = "";
}
