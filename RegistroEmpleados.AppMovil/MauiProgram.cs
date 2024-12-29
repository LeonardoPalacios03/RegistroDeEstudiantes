using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEmpleados.Modelos.Modelos;

namespace RegistroEmpleados.AppMovil
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
            ActualizarCargos();
            ActualizarEmpleados();
            return builder.Build();


        }

        public static async Task ActualizarCargos()
        {
            FirebaseClient client = new FirebaseClient("https://registrodealmunos123-default-rtdb.firebaseio.com/");

            var cargos = await client.Child("Cargos").OnceAsync<Cargo>();

            if (cargos.Count == 0)
            {
               await  client.Child("Cargos").PostAsync(new Cargo { Nombre = "primero medio" });
                await client.Child("Cargos").PostAsync(new Cargo { Nombre = "Segundo medio" });
                await client.Child("Cargos").PostAsync(new Cargo { Nombre = "terceroo medio" });
                await client.Child("Cargos").PostAsync(new Cargo { Nombre = "cuarto medio" });
            }
            else
            {
                foreach (var cargo in cargos)
                {
                    if (cargo.Object.Estado == null)
                    {
                        var cargoActualizado = cargo.Object;
                        cargoActualizado.Estado = true;

                        await client.Child("Cargos").Child(cargo.Key).PutAsync(cargoActualizado);
                    }
                }
            }
        }

        public static async Task ActualizarEmpleados() 
        {
            FirebaseClient client = new FirebaseClient("https://registrodealmunos123-default-rtdb.firebaseio.com/");

            var empleados = await client.Child("Empleados").OnceAsync<Estudiante>();

            foreach (var empleado in empleados)
            {
                if (empleado.Object.Estado == null)
                {
                    var empleadoActualizado = empleado.Object;
                    empleadoActualizado.Estado = true;

                    await client.Child("Empleados").Child(empleado.Key).PutAsync(empleadoActualizado);
                }
            }

        }
    }
}
