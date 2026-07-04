using R5.Core.Entidades;
using R5.Core.Servicios;
using Xunit;

namespace R5.Core.Tests;

public class SesionUsuarioTests
{
    private static Usuario UsuarioDePrueba() => new()
    {
        CodUsuario = 99,
        DescUsuario = "Prueba",
        Almacen = new Almacen { CodAlm = 8, DescAlm = "Tienda" }
    };

    [Fact]
    public void MenuValidado_EmpiezaSinValidar()
    {
        var sesion = new SesionUsuario();
        sesion.IniciarSesion(UsuarioDePrueba());
        Assert.False(sesion.MenuValidado(5));
    }

    [Fact]
    public void ValidarMenu_PersisteDuranteLaSesion()
    {
        var sesion = new SesionUsuario();
        sesion.IniciarSesion(UsuarioDePrueba());
        sesion.ValidarMenu(5);
        Assert.True(sesion.MenuValidado(5));
        Assert.False(sesion.MenuValidado(7));
    }

    [Fact]
    public void CerrarSesion_OlvidaLosMenusValidados()
    {
        var sesion = new SesionUsuario();
        sesion.IniciarSesion(UsuarioDePrueba());
        sesion.ValidarMenu(5);
        sesion.CerrarSesion();
        sesion.IniciarSesion(UsuarioDePrueba());
        Assert.False(sesion.MenuValidado(5));
    }

    [Fact]
    public void IniciarSesion_NuevoUsuarioNoHeredaValidaciones()
    {
        var sesion = new SesionUsuario();
        sesion.IniciarSesion(UsuarioDePrueba());
        sesion.ValidarMenu(5);
        sesion.IniciarSesion(UsuarioDePrueba());
        Assert.False(sesion.MenuValidado(5));
    }
}
