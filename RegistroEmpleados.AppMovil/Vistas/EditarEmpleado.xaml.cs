using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;
using System.Collections.ObjectModel;
using System.Security;
using static System.Net.Mime.MediaTypeNames;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class EditarEmpleado : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registrodealmunos123-default-rtdb.firebaseio.com/");
    public List<Cargo> Cargos { get; set; }
    public ObservableCollection<string> ListaCargos { get; set; }= new ObservableCollection<string>();
    private Estudiante empleadoActualizado = new Estudiante();
    private string empleadoId;
    public EditarEmpleado(string idEmpleado)
	{
		InitializeComponent();
        BindingContext=this;
        empleadoId=idEmpleado;
        CargarListaCargos();
        CargarEmpleado(empleadoId);
       
    }

    private async void CargarListaCargos()
    {
        try
        {
            var cargos = await client.Child("Cargos").OnceAsync<Cargo>();
            ListaCargos.Clear();
            foreach (var cargo in cargos)
            {
                ListaCargos.Add(cargo.Object.Nombre);
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", "Error:" +ex.Message, "Ok");
        }
      
    }

    private async void CargarEmpleado(string idEmpleado)
    {
        var empleado= await client.Child("Empleados").Child(idEmpleado).OnceSingleAsync<Estudiante>();

        if (empleado != null)
        {
            EditPrimerNombreEntry.Text = empleado.PrimerNombre;
            EditSegundoNombreEntry.Text = empleado.SegundoNombre;
            EditPrimerApellidoEntry.Text = empleado.PrimerApellido;
            EditSegundoApellidoEntry.Text = empleado.SegundoApellido;
            EditCorreoEntry.Text = empleado.CorreoElectronico;
            EditEdadEntry.Text = empleado.Edad.ToString();
            EditCargoPicker.SelectedItem = empleado.Cargo?.Nombre;
        }
    }

    private async void ActualizarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditPrimerNombreEntry.Text)||
                string.IsNullOrWhiteSpace(EditSegundoNombreEntry.Text)||
                string.IsNullOrWhiteSpace(EditPrimerApellidoEntry.Text)||
                string.IsNullOrWhiteSpace(EditSegundoApellidoEntry.Text)||
                string.IsNullOrWhiteSpace(EditCorreoEntry.Text)||
                string.IsNullOrWhiteSpace(EditEdadEntry.Text)||
                EditCargoPicker.SelectedItem == null) 
            {

                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            if (!EditCorreoEntry.Text.Contains("@")) 
            {
                await DisplayAlert("Error", "El correo electrónico no es válido", "OK");
                return;
            }

            if(!int.TryParse(EditEdadEntry.Text, out int sueldo))
            {
                await DisplayAlert("Error", "La edad ser un número válido", "OK");
                return ;
            }

            if(sueldo <= 0)
            {
                await DisplayAlert("Error", "La edad debe ser mayor a 0", "OK");
                return;
            }

            empleadoActualizado.Id=empleadoId;
            empleadoActualizado.PrimerNombre=EditPrimerNombreEntry.Text.Trim();
            empleadoActualizado.SegundoNombre= EditSegundoNombreEntry.Text.Trim();
            empleadoActualizado.PrimerApellido= EditPrimerApellidoEntry.Text.Trim(); 
            empleadoActualizado.SegundoApellido= EditSegundoApellidoEntry.Text.Trim(); 
            empleadoActualizado.CorreoElectronico=EditCorreoEntry.Text.Trim(); 
            empleadoActualizado.Edad=sueldo;
            empleadoActualizado.Estado = estadoSwitch.IsToggled;
            empleadoActualizado.Cargo=new Cargo { Nombre=EditCargoPicker.SelectedItem.ToString() };

            await client.Child("Empleados").Child(empleadoActualizado.Id).PutAsync(empleadoActualizado);

            await DisplayAlert("Éxito", "El alumno se modifico correctamente", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", "Error" + ex.Message, "OK");
        }
    }
}