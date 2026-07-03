namespace R5.Core.Api;

/// <summary>
/// Añade las cabeceras de autenticación de WebApiRW (X-Device-Id + X-Api-Key)
/// a toda petición saliente. Los endpoints [ApiKeyAnonimo] las ignoran sin problema.
/// </summary>
public sealed class ApiKeyHandler : DelegatingHandler
{
    private readonly CredencialesApi _credenciales;

    public ApiKeyHandler(CredencialesApi credenciales)
    {
        _credenciales = credenciales;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_credenciales.Enrolado)
        {
            request.Headers.TryAddWithoutValidation("X-Device-Id", _credenciales.DeviceId.ToString());
            request.Headers.TryAddWithoutValidation("X-Api-Key", _credenciales.ApiKey);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
