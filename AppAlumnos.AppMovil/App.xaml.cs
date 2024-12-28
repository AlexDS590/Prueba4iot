using AppAlumnos.AppMovil.Vistas;

namespace AppAlumnos.AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ListarAlumnos());
        }
    }
}
