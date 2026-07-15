namespace R5.Core.Entidades;

/// <summary>Cliente de un pedido web (canon R4): ProdSust = admite producto sustitutivo.</summary>
public sealed class ClienteWww
{
    public int NumCliente { get; set; }
    public bool ProdSust { get; set; }
}

/// <summary>Cabecera de preparación de pedido WEB (canon R4, api/PreparacionWww).</summary>
public sealed class CabPreparacionWww
{
    public ClienteWww Cliente { get; set; } = new();
    public Almacen AlmCentral { get; set; } = new();
    public DateTime Fecha { get; set; }
    public int Pedido { get; set; }
    public int Prods { get; set; }
    public int Preparados { get; set; }
    public int Pendientes { get; set; }
}

/// <summary>Línea de preparación de pedido WEB (canon R4).</summary>
public sealed class LinPreparacionWww
{
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public decimal CantPed { get; set; }
    public int Ubicacion { get; set; }
    public decimal CantPrep { get; set; }
}
