namespace R5.Core.Config;

/// <summary>
/// Almacén de secretos (api-key del dispositivo). La implementación es por plataforma:
/// Android → SecureStorage; Windows → DPAPI (la app va sin empaquetar y SecureStorage no aplica).
/// </summary>
public interface IAlmacenSecretos
{
    Task<string?> ObtenerAsync(string clave);
    Task GuardarAsync(string clave, string valor);
    void Eliminar(string clave);
}

public static class ClavesSecretos
{
    public const string ApiKey = "apikey";
}
