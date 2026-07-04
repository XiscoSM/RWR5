namespace R5.Core.Entidades;

/// <summary>Gestión de empresa de un empleado (firmas de documentos; canon R4, api/Gestiones).</summary>
public sealed class GestionEmpresaUser
{
    public int Mov { get; set; }
    public short IdGestion { get; set; }
    public byte TipoGestion { get; set; }
    public DateTime? FechaFirma { get; set; }
    public string DescFirma { get; set; } = "";
    public string DescGestion { get; set; } = "";
    public string DetallesGestion { get; set; } = "";
    public string DetallesGestionUser { get; set; } = "";
    public string TerminosFirma { get; set; } = "";
    public int Usuario { get; set; }
    public string DescUsuario { get; set; } = "";
    public int Responsable { get; set; }
    public string DescResponsable { get; set; } = "";
    public bool Realizada { get; set; }
    public bool EnvioMail { get; set; }
    public string Mail { get; set; } = "";
}

/// <summary>Empleado validado con su saldo de vacaciones (canon R4).</summary>
public sealed class EmpleadoInfo
{
    public int Empleado { get; set; }
    public string Nombre { get; set; } = "";
    public int DiasVacPend { get; set; }
    public int DiasVacPendAnyoAnt { get; set; }
}

/// <summary>
/// Gestión solicitable del catálogo de un tipo (EPI=18, Vacaciones=16, Madisa=2).
/// Sus flags dirigen el asistente de solicitud (detalles, fechas, términos).
/// </summary>
public sealed class GestionEmpresa
{
    public int IdGestion { get; set; }
    public string DescGestion { get; set; } = "";
    public string Detalles { get; set; } = "";
    public string Terminos { get; set; } = "";
    public string TerminosResp { get; set; } = "";
    public string TerminosUser { get; set; } = "";
    public bool PermiteFechaFin { get; set; }
    public bool IntroFechaIni { get; set; }
    public bool EnvioMail { get; set; }
    public bool RellenarAlm { get; set; }
    public string TipoDato { get; set; } = "STRING";
    public bool ConfirmarDato { get; set; }
}

/// <summary>Alta de solicitud de gestión (mismos parámetros que el proc de RW).</summary>
public sealed class GestionSolicitudInsertDTO
{
    public int UsuarioInsert { get; set; }
    public int Empleado { get; set; }
    public short Alm { get; set; }
    public byte TipoGestion { get; set; }
    public int IdGestion { get; set; }
    public string Detalles { get; set; } = "";
    public DateTime FechaIni { get; set; } = DateTime.Today;
    /// <summary>1753-01-01 = sin fecha fin.</summary>
    public DateTime FechaFin { get; set; } = new(1753, 1, 1);
}
