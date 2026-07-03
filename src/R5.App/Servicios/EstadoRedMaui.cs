using R5.Core.Config;

namespace R5.App.Servicios;

/// <summary>Conectividad real del dispositivo vía MAUI Connectivity.</summary>
public sealed class EstadoRedMaui : IEstadoRed, IDisposable
{
    public EstadoRedMaui()
    {
        Connectivity.ConnectivityChanged += AlCambiar;
    }

    public bool HayInternet => Connectivity.NetworkAccess == NetworkAccess.Internet;

    public event EventHandler<bool>? CambioConectividad;

    private void AlCambiar(object? sender, ConnectivityChangedEventArgs e)
        => CambioConectividad?.Invoke(this, e.NetworkAccess == NetworkAccess.Internet);

    public void Dispose() => Connectivity.ConnectivityChanged -= AlCambiar;
}
