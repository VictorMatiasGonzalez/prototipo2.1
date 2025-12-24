using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ConexionEmpleados
    {
        public List<Empleado> listar()
        {
            List<Empleado> lista = new List<Empleado>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            { 

                conexion.ConnectionString = "Server=.\\SQLEXPRESS;Database=PLANILLAFRANCOS_DB;Integrated Security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = " Selecto Id,NombreCompleto FROM Empleados ";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while(lector.Read())
                {
                    Empleado aux = new Empleado();
                    aux.Id = (int)lector["Id"];
                    aux.NombreCompleto = (string)lector["NombreCompleto"];

                    lista.Add(aux);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        



    }
}
