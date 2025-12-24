using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum TipoFranco
    {
        Trabaja,
        Franco,
        Franco66,
        CarpetaMedica,
        CarpetaExtendida,
        Vacaciones,
        Cero,
    }


    public class Empleado
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }


    }
    public class RegistroFranco
    {
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public TipoFranco Tipo { get; set; }
    }


}
