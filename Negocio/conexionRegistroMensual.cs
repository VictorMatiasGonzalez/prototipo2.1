using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using System.Globalization;


namespace Negocio
{
    public class conexionRegistroMensual
    {
        List<RegistroMensual> listaRegistro;
        // public List<RegistroMensual> ListarRegistroOctubre()
        //{
        //  List<RegistroMensual> lista = new List<RegistroMensual>();
        //  SqlConnection conexion = new SqlConnection();
        // SqlCommand comando = new SqlCommand();
        //  SqlDataReader lector;

        //    try
        //    {
        //        conexion.ConnectionString = "Server=.\\SQLEXPRESS;Database=PLANILLAFRANCOS_DB;Integrated Security=true";
        //        comando.CommandType = System.Data.CommandType.Text;
        //        comando.CommandText = "SELECT * FROM registro_octubre_2025";
        //        comando.Connection = conexion;

        //        conexion.Open();
        //        lector = comando.ExecuteReader();

        //        while (lector.Read())
        //        {
        //            RegistroMensual reg = new RegistroMensual();
        //            reg.IdEmpleado = (int)lector["id_persona"];
        //            reg.NombreCompleto = (string)lector["nombre"];

        //            for (int i = 1; i <= 31; i++)
        //            {
        //                string nombreColumna = $"dia_{i:D2}";
        //                string valor = lector[nombreColumna]?.ToString() ?? "Trabaja";
        //                typeof(RegistroMensual)
        //                    .GetProperty($"Dia_{i:D2}")
        //                    .SetValue(reg, RegistroMensual.InterpretarTipo(valor));

        //            }

        //            lista.Add(reg);
        //        }

        //        conexion.Close();
        //        return lista;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        // }
        public void insertarEnfermero(string nombre, string nombreTabla)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.primeraConexion();
            datos.setearConsulta($"INSERT INTO {nombreTabla} (nombre) VALUES (@nombre)");
            datos.setearParametros("@nombre", nombre);
            datos.ejecutarAccion();

        }
        public List<RegistroMensual> obtenerRegistro(int mes, int año)
        {
            string nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new CultureInfo("es-ES"));
            string nombreTabla = $"registro_{nombreMes.ToLower()}_{año}";
            AccesoDatos datos = new AccesoDatos();
            datos.primeraConexion();
            datos.setearConsulta($"SELECT * FROM { nombreTabla}");
            datos.ejecutarLectura();
            List<RegistroMensual> lista = new List<RegistroMensual>();
            while (datos.Lector.Read())
            {
                RegistroMensual reg = new RegistroMensual();
                reg.IdEmpleado = (int)datos.Lector["id_persona"];
                reg.NombreCompleto = (string)datos.Lector["nombre"];


                for (int i = 1; i <= 31; i++)
                {
                    string nombreColumna = $"dia_{i:D2}";
                    string valor = datos.Lector[nombreColumna]?.ToString() ?? "Trabaja";

                    typeof(RegistroMensual)
                        .GetProperty($"Dia_{i:D2}")
                        ?.SetValue(reg, RegistroMensual.InterpretarTipo(valor));
                }
                lista.Add(reg);
            }
            return lista;
        }
        public void CrearTablaMensualSiNoExiste(int mes, int año)
        {
            string nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new CultureInfo("es-ES")).ToLower();
            string nombreTabla = $"registro_{nombreMes}_{año}";
            AccesoDatos datos = new AccesoDatos();
            datos.primeraConexion();
            datos.setearConsulta($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{nombreTabla}'");//busca el nombre de la tabla que seleccionamos 
            //si tabla existe=1 sino =0.
            datos.ejecutarLectura();

            int cantidad = 0;
            bool existe = false;

            if (datos.Lector.Read())
            {
                cantidad = Convert.ToInt32(datos.Lector[0]);
                existe = cantidad > 0;// si existe true= cantidad >0, si existe no hay que hacer una nueva tabla
            }
            if (!existe)
            {
                try
                {
                    string columnasDias = string.Join(", ", Enumerable.Range(1, 31).Select(i => $"dia_{i:D2} NVARCHAR(20)"));
                    string columnasFinales = $"id_persona INT IDENTITY(1,1) PRIMARY KEY, nombre NVARCHAR(100), mes INT, año INT, {columnasDias}";
                    //string crearTabla = $@"CREATE TABLE {nombreTabla} ( id_persona INT,nombre NVARCHAR(100),{columnasDias})";
                    string crearTabla = $@"CREATE TABLE {nombreTabla} ({columnasFinales})";


                    datos.primeraConexion();
                    datos.setearConsulta(crearTabla);
                    datos.ejecutarAccion();

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            datos.cerrarConexion();

        }
        public bool ExistenRegistrosParaMes(int mes, int año)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.primeraConexion();
                string nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new CultureInfo("es-ES")).ToLower();
                string nombreTabla = $"registro_{nombreMes}_{año}";
                datos.setearConsulta($"SELECT COUNT(*) FROM {nombreTabla}");


                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = Convert.ToInt32(datos.Lector[0]); //convierte ese valor (que viene como tipo object ) a un int para poder usarlo como número

                    if (cantidad > 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;


            }
            finally
            {
                datos.cerrarConexion();
            }


        }
        public List<RegistroMensual> GenerarRegistrosBaseDesdeTablaMensual(string tabla)
        {
            List<RegistroMensual> listaRegistro = new List<RegistroMensual>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.primeraConexion();
                datos.setearConsulta($"SELECT id_persona, nombre FROM {tabla}");
                datos.ejecutarLectura();
                Console.WriteLine("Ejecutando SELECT sobre tabla: " + tabla);


                while (datos.Lector.Read())
                {
                    RegistroMensual registro = new RegistroMensual();
                    registro.IdEmpleado = Convert.ToInt32(datos.Lector["id_persona"]);
                    registro.NombreCompleto = datos.Lector["nombre"].ToString();
                    Console.WriteLine("Leyendo: " + datos.Lector["id_persona"] + " - " + datos.Lector["nombre"]);
                    // Inicializar días en blanco
                    for (int d = 1; d <= 31; d++)
                    {
                        string prop = $"Dia_{d:D2}";
                        var propiedad = typeof(RegistroMensual).GetProperty(prop);
                        if (propiedad != null)
                        {
                            try
                            {
                                propiedad.SetValue(registro, TipoFranco.Trabaja.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error en propiedad {prop}: " + ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Propiedad {prop} no encontrada");


                        }




                    }

                    listaRegistro.Add(registro);


                }

                return listaRegistro;
            }
            catch
            {
                return new List<RegistroMensual>();
            }
            finally
            {
                datos.cerrarConexion();
            }


        }
        public void InsertarRegistrosBaseEnTablaMensual(List<RegistroMensual> registros, int mes, int año)
        {
            if (registros == null || registros.Count == 0)
                return;

            string nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new CultureInfo("es-ES")).ToLower();
            string nombreTabla = $"registro_{nombreMes}_{año}";

            try
            {
                foreach (var registro in registros)
                {
                    using (AccesoDatos datos = new AccesoDatos())
                    {
                        datos.primeraConexion();

                        string consulta = $@"INSERT INTO {nombreTabla} (nombre, mes, año)
                                     VALUES (@nombre, @mes, @año)";
                        datos.setearConsulta(consulta);
                        
                        datos.setearParametros("@nombre", registro.NombreCompleto);
                        datos.setearParametros("@mes", mes);
                        datos.setearParametros("@año", año);
                        datos.ejecutarAccion();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }


        }
        public void eliminar (int id, string nombreTabla)
        {
             AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.primeraConexion();
                datos.setearConsulta($"DELETE FROM {nombreTabla} WHERE id_persona=@Id");
                datos.setearParametros("@id", id);
                datos.ejecutarAccion();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
           
        }
        public void updateEnfermero(string nombre, int id, string nombreTabla )//seguimos aca
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.primeraConexion();
                datos.setearConsulta($"UPDATE {nombreTabla} SET nombre=@nombre WHERE id_persona=@Id");
                datos.setearParametros("@nombre", nombre);
                datos.setearParametros("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void CrearTablaMensualConIdentidadSiNoExiste(int mes, int año)
        {
            string nombreMes = new DateTime(año, mes, 1).ToString("MMMM", new CultureInfo("es-ES")).ToLower();
            string nombreTabla = $"registro_{nombreMes}_{año}";

            string columnasDias = string.Join(", ", Enumerable.Range(1, 31).Select(i => $"dia_{i:D2} NVARCHAR(20)"));
            string columnasFinales = $"id_persona INT IDENTITY(1,1) PRIMARY KEY, nombre NVARCHAR(100), mes INT, año INT, {columnasDias}";

            string crearTabla = $@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{nombreTabla}' AND xtype='U')
        BEGIN
            CREATE TABLE {nombreTabla} ({columnasFinales})
        END";

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.primeraConexion();
                datos.setearConsulta(crearTabla);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }

}
