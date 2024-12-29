using Firebase.Database;
using Firebase.Database.Query;
using AppAlumnos.Modelos.Modelos;

namespace AppAlumnos.AppMovil.Vistas;

public partial class EditarAlumno : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://alumnosprueba-a7b1b-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; }
    private string alumnoId;
    private Alumno alumnoActual;

    public EditarAlumno(Alumno alumno)
    {
        InitializeComponent();
        alumnoId = alumno.Id;
        alumnoActual = alumno;
        BindingContext = this;
        InicializarDatos();
    }

    private async void InicializarDatos()
    {
        await CargarCursos();
        CargarAlumno();
    }

    private async Task CargarCursos()
    {
        try
        {
            var cursos = await client.Child("Cursos").OnceAsync<Curso>();
            Cursos = cursos.Select(x => x.Object).ToList();
            OnPropertyChanged(nameof(Cursos));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los cursos: " + ex.Message, "OK");
        }
    }

    private void CargarAlumno()
    {
        primerNombreEntry.Text = alumnoActual.PrimerNombre;
        segundoNombreEntry.Text = alumnoActual.SegundoNombre;
        primerApellidoEntry.Text = alumnoActual.PrimerApellido;
        segundoApellidoEntry.Text = alumnoActual.SegundoApellido;
        correoEntry.Text = alumnoActual.Correo;
        edadEntry.Text = alumnoActual.Edad.ToString();
        fechaInicioPicker.Date = alumnoActual.FechaInicio;

        var cursoSeleccionado = Cursos.FirstOrDefault(c => c.Nombre == alumnoActual.CursoSeleccionado?.Nombre);
        if (cursoSeleccionado != null)
        {
            CursoPicker.SelectedItem = cursoSeleccionado;
        }
    }

    private async void actualizarButton_Clicked(object sender, EventArgs e)
    {
        if (CursoPicker.SelectedItem is not Curso cursoSeleccionado)
        {
            await DisplayAlert("Error", "Debe seleccionar un curso", "OK");
            return;
        }

        try
        {
            alumnoActual.PrimerNombre = primerNombreEntry.Text;
            alumnoActual.SegundoNombre = segundoNombreEntry.Text;
            alumnoActual.PrimerApellido = primerApellidoEntry.Text;
            alumnoActual.SegundoApellido = segundoApellidoEntry.Text;
            alumnoActual.Correo = correoEntry.Text;
            alumnoActual.Edad = int.TryParse(edadEntry.Text, out var edad) ? edad : 0;
            alumnoActual.FechaInicio = fechaInicioPicker.Date;
            alumnoActual.CursoSeleccionado = cursoSeleccionado;

            await client.Child("Alumnos").Child(alumnoId).PutAsync(alumnoActual);
            await DisplayAlert("Éxito", "Alumno actualizado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo actualizar el alumno: " + ex.Message, "OK");
        }
    }
}
