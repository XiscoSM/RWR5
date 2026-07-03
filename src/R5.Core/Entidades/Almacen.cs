namespace R5.Core.Entidades;

/// <summary>Almacén. Propiedades = contrato de WebApiRW (canon R4).</summary>
public sealed class Almacen
{
    public short CodAlm { get; set; }
    public string DescAlm { get; set; } = "";
    public byte Tipo { get; set; }
    public bool SoloConStock { get; set; }
    public byte NumDigitosPasillo { get; set; }
    public bool ControlBultos { get; set; }
    public byte CentroDistEmitirPedidos { get; set; }
    public byte CentroDistRecibirPedidos { get; set; }
    public bool PedCentralStockMax { get; set; }
    public bool NoIntroEanPrep { get; set; }
    public bool NoIntroEanCompra { get; set; }
    public byte PatronDatosCompra { get; set; }
    public string? RegistroSanitario { get; set; }
    public bool AjustesContraOtroAlm { get; set; }
    public bool AlmWeb { get; set; }
    public short AlmWebAsoc { get; set; }
    public byte RecibirPedCentral { get; set; }
}
