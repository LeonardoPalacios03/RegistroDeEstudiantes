using RegistroEmpleados.AppMovil.Vistas;

namespace RegistroEmpleados.AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new ListarEmpleados());
        }
    }
}
