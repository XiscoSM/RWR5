namespace R5.Core.Entidades;

/// <summary>Estados del pedido de cliente a central (los recalcula la BD).</summary>
public static class EstadoPedidoCliente
{
    public const byte Abierto = 0;
    public const byte EnviadoCentral = 1;
    public const byte EnPreparacion = 2;
    public const byte EnReparto = 3;
    public const byte EntregadoCliente = 4;
    public const byte Anulado = 5;

    public static string DescDoc(byte estado) => estado switch
    {
        0 => "Abierto", 1 => "Enviado a central", 2 => "En preparación",
        3 => "En reparto", 4 => "Entregado", 5 => "Anulado", _ => $"Estado {estado}"
    };

    public static string DescLinea(byte estado) => estado switch
    {
        0 => "Pendiente", 1 => "Preparación", 2 => "En reparto",
        3 => "Entregado", 4 => "No disponible", 5 => "Anulada", _ => $"Estado {estado}"
    };
}

/// <summary>Cabecera del pedido de cliente a central (módulo 17).</summary>
public sealed class PedidoClienteCab
{
    public int Pedido { get; set; }
    public DateTime? Fecha { get; set; }
    public byte EstadoDoc { get; set; }
    public int Empleado { get; set; }
    public short Alm { get; set; }
    public short AlmCentral { get; set; }
    public long Telefono { get; set; }
    public string NombreCliente { get; set; } = "";
    public DateTime? FechaHoraEntrega { get; set; }
    public string Observaciones { get; set; } = "";
    public string EstadoDesc { get; set; } = "";
    public string DescAlm { get; set; } = "";
    public int Lineas { get; set; }

    public string DescEstado => EstadoDesc.Length > 0 ? EstadoDesc : EstadoPedidoCliente.DescDoc(EstadoDoc);
    public bool Abierto => EstadoDoc == EstadoPedidoCliente.Abierto;
    public bool Vivo => EstadoDoc is not (EstadoPedidoCliente.EntregadoCliente or EstadoPedidoCliente.Anulado);
}

/// <summary>Línea del pedido / fila de la bandeja del preparador (módulo 17).</summary>
public sealed class PedidoClienteLin
{
    public int Pedido { get; set; }
    public int Linea { get; set; }
    public byte Estado { get; set; }
    public string EstadoDesc { get; set; } = "";
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public decimal Cant { get; set; }
    public bool DecimalesEnCant { get; set; }
    public DateTime? FechaPedCentral { get; set; }
    public int PedidoCentral { get; set; }
    public string Observaciones { get; set; } = "";
    public short Alm { get; set; }
    public short AlmCentral { get; set; }
    public string DescAlm { get; set; } = "";
    public string NombreCliente { get; set; } = "";
    public long Telefono { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHoraEntrega { get; set; }

    public string DescEstado => EstadoDesc.Length > 0 ? EstadoDesc : EstadoPedidoCliente.DescLinea(Estado);
    public bool Atrasada => FechaHoraEntrega is not null && FechaHoraEntrega < DateTime.Now
        && Estado is < 3;
}

public sealed class PedidoClienteCabDTO
{
    public int Empleado { get; set; }
    public short Alm { get; set; }
    public short AlmCentral { get; set; }
    public long Telefono { get; set; }
    public string NombreCliente { get; set; } = "";
    public DateTime FechaHoraEntrega { get; set; }
    public string Observaciones { get; set; } = "";
}

public sealed class PedidoClienteLinDTO
{
    public int Prod { get; set; }
    public decimal Cant { get; set; }
    public string Observaciones { get; set; } = "";
}

/// <summary>Programa con teclas Madisa (selector de producto del canon R3).</summary>
public sealed class ProgramaMadisa
{
    public short Prog { get; set; }
    public string DescProg { get; set; } = "";
}

/// <summary>Tecla Madisa de un programa.</summary>
public sealed class TeclaMadisa
{
    public short Tecla { get; set; }
    public string DescTecla { get; set; } = "";
    public string DescDptoDibal { get; set; } = "";
}

/// <summary>Producto de una tecla Madisa disponible en el almacén del pedido.</summary>
public sealed class ProductoMadisa
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public long Ean { get; set; }
}
