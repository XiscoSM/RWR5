namespace R5.Core.Entidades;

/// <summary>Sección con horario (canon R4, api/Horarios/Secciones).</summary>
public sealed class HorarioSeccion
{
    public byte Seccion { get; set; }
    public string DescSeccion { get; set; } = "";
}

/// <summary>Fila del cuadro semanal: empleado + celdas ya en texto.</summary>
public sealed class HorarioSemanaFila
{
    public string Nombre { get; set; } = "";
    public List<string> Celdas { get; set; } = new();
}

/// <summary>Cuadro semanal de una sección (columnas dinámicas del proc).</summary>
public sealed class HorarioSemana
{
    public List<string> Columnas { get; set; } = new();
    public List<HorarioSemanaFila> Filas { get; set; } = new();
}
