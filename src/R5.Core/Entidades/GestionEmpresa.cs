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
