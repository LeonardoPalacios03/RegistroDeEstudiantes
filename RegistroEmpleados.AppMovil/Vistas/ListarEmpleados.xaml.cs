using Firebase.Database;
using Firebase.Database.Query;
using LiteDB;
using RegistroEmpleados.Modelos.Modelos;
using System.Collections.ObjectModel;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class ListarEmpleados : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registrodealmunos123-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();
    public ListarEmpleados()
    {
        InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    private async void CargarLista()
    {
        Lista.Clear();
        var empleados = await client.Child("Empleados").OnceAsync<Estudiante>();

        var empleadosActivos= empleados.Where(e=>e.Object.Estado==true).ToList();

        foreach (var empleado in empleadosActivos)
        {
            Lista.Add(new Estudiante
            {
                Id=empleado.Key,
                PrimerNombre= empleado.Object.PrimerNombre,
                SegundoNombre= empleado.Object.SegundoNombre,
                PrimerApellido= empleado.Object.PrimerApellido,
                SegundoApellido= empleado.Object.SegundoApellido,
                CorreoElectronico= empleado.Object.CorreoElectronico,
                Edad= empleado.Object.Edad,
                FechaInicio=empleado.Object.FechaInicio,
                Estado= empleado.Object.Estado,
                Cargo= empleado.Object.Cargo
            });
        }
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();

        if (filtro.Length > 0)
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEmpleadoBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEmpleado());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var empleado = boton?.CommandParameter as Estudiante;

        if (empleado != null &&!string.IsNullOrEmpty(empleado.Id))
        {
            await Navigation.PushAsync(new EditarEmpleado(empleado.Id));
        }
        else 
        {
            await DisplayAlert("Error", "No se pudo obtener la información del estudiante", "OK");
        }
    }

    private async void deshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var empleado = boton?.CommandParameter as Estudiante;

        if (empleado == null) 
        {
            await DisplayAlert("Error", "No se pudo obtener la información del estudiante", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert
            ("Confirmación", $"Está seguro que desea deshabilitar al estudiante {empleado.NombreCompleto}", "Sí", "No");

        if (confirmacion)
        {
            try
            {
                empleado.Estado = false;
                await client.Child("Empleados").Child(empleado.Id).PutAsync(empleado);
                await DisplayAlert("Exito", $"Se ha deshabilitado correctamente al usuario {empleado.NombreCompleto}", "OK");
                CargarLista();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}