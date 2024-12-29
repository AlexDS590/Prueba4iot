using System.Collections.ObjectModel;
using System.Linq;
using AppAlumnos.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;

namespace AppAlumnos.AppMovil.Vistas;

public partial class ListarAlumnos : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://alumnosprueba-a7b1b-default-rtdb.firebaseio.com/");
    public ObservableCollection<Alumno> Lista { get; set; } = new ObservableCollection<Alumno>();

    public ListarAlumnos()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAlumnos();
    }

    private async Task CargarAlumnos()
    {
        try
        {
            var alumnos = await client.Child("Alumnos").OnceAsync<Alumno>();
            Lista.Clear();

            foreach (var alumno in alumnos)
            {
                if (alumno.Object.Estado == true)
                {
                    alumno.Object.Id = alumno.Key;
                    Lista.Add(alumno.Object);
                }
            }

            ListarCollection.ItemsSource = Lista;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los alumnos: " + ex.Message, "OK");
        }
    }

    private void filtroSerachBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSerachBar.Text?.ToLower() ?? string.Empty;
        ListarCollection.ItemsSource = string.IsNullOrEmpty(filtro)
            ? Lista
            : Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
    }

    private async void AgregarAlumnoButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearAlumno());
    }

    private async void DeshabilitarAlumno_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var alumno = boton?.CommandParameter as Alumno;

        if (alumno != null && !string.IsNullOrEmpty(alumno.Id))
        {
            bool confirmacion = await DisplayAlert("Confirmar", $"¿Desea deshabilitar a {alumno.NombreCompleto}?", "Sí", "No");

            if (confirmacion)
            {
                alumno.Estado = false;
                await client.Child("Alumnos").Child(alumno.Id).PutAsync(alumno);
                Lista.Remove(alumno);
                ListarCollection.ItemsSource = Lista;
            }
        }
    }

    private async void EditarAlumno_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var alumno = boton?.CommandParameter as Alumno;

        if (alumno != null && !string.IsNullOrEmpty(alumno.Id))
        {
            await Navigation.PushAsync(new EditarAlumno(alumno));
        }
    }
}
