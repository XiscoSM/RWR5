using R5.UI;
using Xunit;

namespace R5.Core.Tests;

public class FormatoTests
{
    [Fact]
    public void Moneda_FormateaConDosDecimalesYComa()
        => Assert.Equal("12,50 €", Formato.Moneda(12.5m));

    [Fact]
    public void Moneda_NuloEsGuion()
        => Assert.Equal("—", Formato.Moneda(null));

    [Theory]
    [InlineData(false, "3")]     // unidades: sin decimales de relleno
    [InlineData(true, "3,000")]  // peso: siempre 3 decimales
    public void Cantidad_RespetaDecimalesEnCant(bool decimales, string esperado)
        => Assert.Equal(esperado, Formato.Cantidad(3m, decimales));

    [Fact]
    public void Cantidad_UnidadesRecortaCerosPeroNoDecimalesReales()
        => Assert.Equal("3,25", Formato.Cantidad(3.25m, decimales: false));

    [Fact]
    public void Fecha_AnteriorA1950EsSinFecha()
        => Assert.Equal("—", Formato.Fecha(new DateTime(1900, 1, 1)));

    [Fact]
    public void Fecha_NormalSaleCorta()
        => Assert.Equal("04/07/26", Formato.Fecha(new DateTime(2026, 7, 4)));
}
