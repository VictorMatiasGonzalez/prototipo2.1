using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class RegistroMensual
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; }

        
        public TipoFranco Dia_01 { get; set; }
        public TipoFranco Dia_02 { get; set; }
        public TipoFranco Dia_03 { get; set; }
        public TipoFranco Dia_04 { get; set; }
        public TipoFranco Dia_05 { get; set; }
        public TipoFranco Dia_06 { get; set; }
        public TipoFranco Dia_07 { get; set; }
        public TipoFranco Dia_08 { get; set; }
        public TipoFranco Dia_09 { get; set; }
        public TipoFranco Dia_10 { get; set; }
        public TipoFranco Dia_11 { get; set; }
        public TipoFranco Dia_12 { get; set; }
        public TipoFranco Dia_13 { get; set; }
        public TipoFranco Dia_14 { get; set; }
        public TipoFranco Dia_15 { get; set; }
        public TipoFranco Dia_16 { get; set; }
        public TipoFranco Dia_17 { get; set; }
        public TipoFranco Dia_18 { get; set; }
        public TipoFranco Dia_19 { get; set; }
        public TipoFranco Dia_20 { get; set; }
        public TipoFranco Dia_21 { get; set; }
        public TipoFranco Dia_22 { get; set; }
        public TipoFranco Dia_23 { get; set; }
        public TipoFranco Dia_24 { get; set; }
        public TipoFranco Dia_25 { get; set; }
        public TipoFranco Dia_26 { get; set; }
        public TipoFranco Dia_27 { get; set; }
        public TipoFranco Dia_28 { get; set; }
        public TipoFranco Dia_29 { get; set; }
        public TipoFranco Dia_30 { get; set; }
        public TipoFranco Dia_31 { get; set; }

        public static TipoFranco InterpretarTipo(string valor)
        {
            if (Enum.TryParse(valor, out TipoFranco tipo))
                return tipo;
            return TipoFranco.Trabaja;
            
        }



        
    }

}
