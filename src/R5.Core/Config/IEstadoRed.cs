namespace R5.Core.Config;

/// <summary>
/// Estado de conectividad del dispositivo. Implementado por el host MAUI
/// (Connectivity); la UI lo usa para el aviso de "sin conexión".
/// </summary>
public interface IEstadoRed
{
    bool HayInternet { get; }
    event EventHandler<bool>? CambioConectividad;
}
