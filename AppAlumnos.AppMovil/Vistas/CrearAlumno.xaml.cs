using AppAlumnos.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;

namespace AppAlumnos.AppMovil.Vistas
{
    public partial class CrearAlumno : ContentPage
    {
        FirebaseClient client = new FirebaseClient("https://alumnosprueba-a7b1b-default-rtdb.firebaseio.com/");

        public List<Curso> Cursos { get; set; }
        public CrearAlumno()
        {
            InitializeComponent();
            ListarCursos();
            BindingContext = this;
        }

        private void ListarCursos()
        {
            var cursos = client.Child("Cursos").OnceAsync<Curso>();
            Cursos = cursos.Result.Select(x=>x.Object).ToList();  
        }

        private async void guardarButton_Clicked(object sender, EventArgs e)
        {
            Curso curso = CursoPicker.SelectedItem as Curso;

            var alumno = new Alumno
            {
                PrimerNombre = primerNombreEntry.Text,
                SegundoNombre = segundoNombreEntry.Text,
                PrimerApellido = primerApellidoEntry.Text,
                SegundoApellido = segundoApellidoEntry.Text,
                Correo = correoEntry.Text,
                Edad = int.Parse(edadEntry.Text),
                CursoSeleccionado = curso,
                FechaInicio = fechaInicioPicker.Date,
            };

            try
            {
                await client.Child("Alumnos").PostAsync(alumno);
                await DisplayAlert("Éxito", $"El alumno {alumno.PrimerNombre} {alumno.PrimerApellido} fue guardado correctamente", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
