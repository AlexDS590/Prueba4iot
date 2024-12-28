using System;

namespace AppAlumnos.Modelos.Modelos
{
    public class Alumno
    {
        public string Id { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public Curso CursoSeleccionado { get; set; }
        public DateTime FechaInicio { get; set; }
        public bool Estado { get; set; } = true;
        public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
    }
}
