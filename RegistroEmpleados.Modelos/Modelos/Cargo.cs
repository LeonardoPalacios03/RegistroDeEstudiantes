using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEmpleados.Modelos.Modelos
{
    public class Cargo
    {
        public string? Nombre { get; set; }

        public bool? Estado { get; set; }
    }
}
