namespace R5.Core.Api;

/// <summary>
/// Credenciales de dispositivo en memoria (X-Device-Id + X-Api-Key).
/// Se cargan al arrancar desde el almacén seguro y las actualiza el enrolamiento.
/// </summary>
public sealed class CredencialesApi
{
    /// <summary>IdHard del dispositivo (clave de axe.Hardware_HM1).</summary>
    public int DeviceId { get; private set; }

    public string? ApiKey { get; private set; }

    public bool Enrolado => DeviceId > 0 && !string.IsNullOrEmpty(ApiKey);

    public void Establecer(int deviceId, string? apiKey)
    {
        DeviceId = deviceId;
        ApiKey = apiKey;
    }

    public void Limpiar() => Establecer(0, null);
}
