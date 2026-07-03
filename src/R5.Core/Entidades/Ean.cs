namespace R5.Core.Entidades;

/// <summary>Código de barras de un producto (canon R4). UnidCaja > 1 = EAN de caja.</summary>
public sealed class Ean
{
    public long? CodEan { get; set; }
    public short UnidCaja { get; set; } = 1;

    public bool EsCaja => UnidCaja > 1;
}
