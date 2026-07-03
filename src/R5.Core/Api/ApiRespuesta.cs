using System.Net;

namespace R5.Core.Api;

/// <summary>
/// Resultado de una llamada a WebApiRW. Sustituye el patrón bool+Result de RWR4
/// por un objeto único: la página comprueba Ok y, si falla, muestra Error.
/// </summary>
public class ApiRespuesta
{
    public bool Ok { get; init; }

    /// <summary>Mensaje de error legible para el usuario (vacío si Ok).</summary>
    public string Error { get; init; } = "";

    public HttpStatusCode? Codigo { get; init; }

    /// <summary>True si el fallo es de red/timeout (no del servidor): la UI ofrece reintentar.</summary>
    public bool SinConexion { get; init; }

    public static ApiRespuesta Exito(HttpStatusCode codigo) => new() { Ok = true, Codigo = codigo };
    public static ApiRespuesta Fallo(string error, HttpStatusCode? codigo = null, bool sinConexion = false)
        => new() { Ok = false, Error = error, Codigo = codigo, SinConexion = sinConexion };
}

/// <summary>Resultado con cuerpo deserializado. Si Ok, Datos contiene la respuesta.</summary>
public sealed class ApiRespuesta<T> : ApiRespuesta
{
    public T? Datos { get; init; }

    public static ApiRespuesta<T> Exito(T datos, HttpStatusCode codigo)
        => new() { Ok = true, Datos = datos, Codigo = codigo };

    public static new ApiRespuesta<T> Fallo(string error, HttpStatusCode? codigo = null, bool sinConexion = false)
        => new() { Ok = false, Error = error, Codigo = codigo, SinConexion = sinConexion };
}
