using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class diaVisual
    {
        public DateTime Fecha { get; set; }
        public string TextoVisual { get; set; }

        public diaVisual(DateTime fecha, string textoVisual)// pasa fecha en nuemros y palabras y las integra en el metodo
        {
            Fecha = fecha;
            TextoVisual = textoVisual;
        }


    }
}
