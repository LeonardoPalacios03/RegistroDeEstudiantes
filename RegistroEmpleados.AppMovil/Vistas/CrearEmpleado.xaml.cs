using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class CrearEmpleado : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registrodealmunos123-default-rtdb.firebaseio.com/");
    public List<Cargo> Cargos { get; set; }
    public CrearEmpleado()
	{
        InitializeComponent();
        ListarCargos();
        BindingContext = this;
    }

    private void ListarCargos()
    {
        var cargos = client.Child("Cargos").OnceAsync<Cargo>();
        Cargos = cargos.Result.Select(x => x.Object).ToList();
    }

    private async void guardarBoton_Clicked(object sender, EventArgs e)
    {
        Cargo cargo = cargoPicker.SelectedItem as Cargo;

        var empleado = new Estudiante
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            FechaInicio = fechaInicioPicker.Date,
            Edad = int.Parse(edadEntry.Text),
            Cargo = cargo
        };

        try
        {
            await client.Child("Empleados").PostAsync(empleado);
            await DisplayAlert("Éxito", $"El estudiante {empleado.PrimerNombre} {empleado.PrimerApellido} fue guardado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}