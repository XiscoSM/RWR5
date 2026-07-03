using R5.Core.Config;

namespace R5.App.Servicios;

/// <summary>
/// Almacén de secretos por plataforma:
/// - Android: SecureStorage (Keystore).
/// - Windows: la app va sin empaquetar (WindowsPackageType=None) y SecureStorage no está
///   disponible, así que se cifra con DPAPI (usuario actual) a archivo en AppDataDirectory.
/// </summary>
public sealed class AlmacenSecretosMaui : IAlmacenSecretos
{
#if WINDOWS
    private static string Ruta(string clave)
        => Path.Combine(FileSystem.AppDataDirectory, $"secreto_{clave}.bin");

    public Task<string?> ObtenerAsync(string clave)
    {
        string ruta = Ruta(clave);
        if (!File.Exists(ruta)) return Task.FromResult<string?>(null);

        byte[] cifrado = File.ReadAllBytes(ruta);
        byte[] claro = System.Security.Cryptography.ProtectedData.Unprotect(
            cifrado, optionalEntropy: null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
        return Task.FromResult<string?>(System.Text.Encoding.UTF8.GetString(claro));
    }

    public Task GuardarAsync(string clave, string valor)
    {
        byte[] cifrado = System.Security.Cryptography.ProtectedData.Protect(
            System.Text.Encoding.UTF8.GetBytes(valor), optionalEntropy: null,
            System.Security.Cryptography.DataProtectionScope.CurrentUser);
        File.WriteAllBytes(Ruta(clave), cifrado);
        return Task.CompletedTask;
    }

    public void Eliminar(string clave)
    {
        string ruta = Ruta(clave);
        if (File.Exists(ruta)) File.Delete(ruta);
    }
#else
    public async Task<string?> ObtenerAsync(string clave)
        => await SecureStorage.Default.GetAsync(clave);

    public Task GuardarAsync(string clave, string valor)
        => SecureStorage.Default.SetAsync(clave, valor);

    public void Eliminar(string clave)
        => SecureStorage.Default.Remove(clave);
#endif
}
