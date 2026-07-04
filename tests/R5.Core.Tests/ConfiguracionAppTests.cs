using R5.Core.Config;
using Xunit;

namespace R5.Core.Tests;

public class ConfiguracionAppTests
{
    [Theory]
    [InlineData(EntornoApi.Produccion, "https://webapirw.hiperc.es/webapirw4/api")]
    [InlineData(EntornoApi.Beta, "https://webapirw.hiperc.es/webapirw4beta/api")]
    [InlineData(EntornoApi.Local, "https://localhost:7049/api")]
    public void UrlApi_SeDerivaDelEntorno(EntornoApi entorno, string esperada)
    {
        var config = new ConfiguracionApp { Entorno = entorno };
        Assert.Equal(esperada, config.UrlApi);
    }

    [Fact]
    public void UrlApi_PersonalizadoUsaLaUrlDelUsuario()
    {
        var config = new ConfiguracionApp
        {
            Entorno = EntornoApi.Personalizado,
            UrlPersonalizada = "https://mi-servidor/api"
        };
        Assert.Equal("https://mi-servidor/api", config.UrlApi);
    }

    [Fact]
    public void CopiarDe_TraeTodosLosCampos()
    {
        var origen = new ConfiguracionApp
        {
            Entorno = EntornoApi.Local,
            UrlPersonalizada = "x",
            NumTerminal = 12,
            DeviceId = 7713,
            UltimoUsuario = 99,
            PuertoBalanza = 3
        };
        var destino = new ConfiguracionApp();
        destino.CopiarDe(origen);

        Assert.Equal(EntornoApi.Local, destino.Entorno);
        Assert.Equal("x", destino.UrlPersonalizada);
        Assert.Equal(12, destino.NumTerminal);
        Assert.Equal(7713, destino.DeviceId);
        Assert.Equal(99, destino.UltimoUsuario);
        Assert.Equal(3, destino.PuertoBalanza);
    }
}
