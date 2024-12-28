using AppAlumnos.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;

namespace AppAlumnos.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            CrearAlumno().ConfigureAwait(false);
            return builder.Build();
        }

        public static async Task CrearAlumno()
        {
            FirebaseClient client = new FirebaseClient("https://alumnosprueba-a7b1b-default-rtdb.firebaseio.com/");

            var cursos = await client.Child("Cursos").OnceAsListAsync<Curso>();

            if (cursos.Count == 0)
            {
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "1ro Básico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "2ro Básico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "3ro Básico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "4ro Básico" });
            }
        }
    }
}
