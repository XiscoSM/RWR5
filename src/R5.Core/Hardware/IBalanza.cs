namespace R5.Core.Hardware;

/// <summary>Lectura de peso recibida de la báscula.</summary>
public readonly record struct LecturaPeso(decimal Kilos, bool Estable, DateTime Instante);

/// <summary>
/// Báscula por puerto serie USB, tras interfaz para implementar por plataforma:
/// Windows → System.IO.Ports (se valida primero); Android → usb-serial (fase posterior).
/// </summary>
public interface IBalanza
{
    bool Disponible { get; }
    bool Conectada { get; }

    Task<bool> ConectarAsync(CancellationToken ct = default);
    Task DesconectarAsync();

    /// <summary>Solicita una lectura puntual de peso; null si no hay lectura estable a tiempo.</summary>
    Task<LecturaPeso?> LeerPesoAsync(CancellationToken ct = default);

    /// <summary>Lecturas en continuo mientras está conectada (para pantallas de pesaje en vivo).</summary>
    event EventHandler<LecturaPeso>? PesoRecibido;
}

/// <summary>Implementación nula: la app funciona sin báscula (módulos que no pesan).</summary>
public sealed class BalanzaNula : IBalanza
{
    public bool Disponible => false;
    public bool Conectada => false;
    public Task<bool> ConectarAsync(CancellationToken ct = default) => Task.FromResult(false);
    public Task DesconectarAsync() => Task.CompletedTask;
    public Task<LecturaPeso?> LeerPesoAsync(CancellationToken ct = default) => Task.FromResult<LecturaPeso?>(null);
    public event EventHandler<LecturaPeso>? PesoRecibido { add { } remove { } }
}
