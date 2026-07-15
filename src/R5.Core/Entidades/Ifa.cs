namespace R5.Core.Entidades;

/// <summary>Resultado de un envío transaccional a IFA (carga/mantenimiento).</summary>
public sealed class IfaEnvioResultado
{
    public string Respuesta { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string FechaDatos { get; set; } = "";
    public int Lineas { get; set; }
    public List<string> Errores { get; set; } = new();
}

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
