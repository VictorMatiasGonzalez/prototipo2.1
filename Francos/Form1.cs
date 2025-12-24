using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Globalization;

namespace Francos
{
    public partial class Francos : Form
    {
        private bool cargandoFormulario = true;


        int mesActual = DateTime.Now.Month;
        int añoActual = DateTime.Now.Year;


        public Francos()
        {
            InitializeComponent();
        }

        private void Francos_Load(object sender, EventArgs e)
        {
            cargandoFormulario = true;

            cargar(); 

            for (int i = 1; i <= 12; i++)
            {
                cbMes.Items.Add(i.ToString());
            }

            cbMes.SelectedItem = DateTime.Now.Month.ToString();

            cargandoFormulario = false;


        }
        public void cargar()
        {
            conexionRegistroMensual conexion = new conexionRegistroMensual();
            List<RegistroMensual> registros = conexion.obtenerRegistro(mesActual, añoActual);
            CrearGrillaDesdeRegistros(dgvPlanilla, registros, mesActual, añoActual);
            dgvPlanilla.CellPainting += dgvPlanilla_CellPainting;



        }

        private void CrearGrillaDesdeRegistros(DataGridView dgv, List<RegistroMensual> registros, int mes, int año)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            // 🟢 Columna oculta para el ID
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "id_persona"; // Este nombre lo usás en Cells["id_persona"]
            colId.HeaderText = "ID";
            colId.Visible = false;
            dgv.Columns.Add(colId);

            // 🟢 Columna visible para el nombre
            DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
            colNombre.Name = "IdEmpleado"; // nombre interno
            colNombre.HeaderText = "Empleado"; // título visible
            dgv.Columns.Add(colNombre);



            List<diaVisual> diasVisuales = generarDiasRealesDelMes(mes, año);

            
            for (int d = 1; d <=diasVisuales.Count; d++)
            {
                DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
                col.Name = $"D{d}";
                col.HeaderText = diasVisuales[d - 1].TextoVisual;
                col.Width = 80;

                // 🎨 Colores simples
                DayOfWeek dia = diasVisuales[d - 1].Fecha.DayOfWeek;
                if (dia == DayOfWeek.Saturday)
                    col.DefaultCellStyle.BackColor = Color.Yellow;
                else if (dia == DayOfWeek.Sunday)
                    col.DefaultCellStyle.BackColor = Color.Orange;

                dgv.Columns.Add(col);


            }

            foreach (var reg in registros)
            {
                int rowIndex = dgv.Rows.Add();

                dgv.Rows[rowIndex].Cells["id_persona"].Value = reg.IdEmpleado; // ID oculto
                dgv.Rows[rowIndex].Cells["IdEmpleado"].Value = reg.NombreCompleto;


                for (int d = 1; d <= diasVisuales.Count; d++)
                {
                    string colName = $"D{d}";
                    var comboCell = new DataGridViewComboBoxCell();
                    comboCell.Items.AddRange(Enum.GetNames(typeof(TipoFranco)));

                    var valor = typeof(RegistroMensual).GetProperty($"Dia_{d:D2}").GetValue(reg);
                    comboCell.Value = valor.ToString();

                    dgv.Rows[rowIndex].Cells[colName] = comboCell;



                    DayOfWeek dia = diasVisuales[d - 1].Fecha.DayOfWeek;
                    if (dia == DayOfWeek.Saturday)
                        comboCell.Style.BackColor = Color.Yellow;
                    else if (dia == DayOfWeek.Sunday)
                        comboCell.Style.BackColor = Color.Orange;
                }



                
                for (int d = 1; d <= diasVisuales.Count; d++)
                {
                    DayOfWeek dia = diasVisuales[d - 1].Fecha.DayOfWeek;
                    if (dia == DayOfWeek.Saturday)
                        dgv.Rows[rowIndex].Cells[d].Style.BackColor = Color.Yellow;
                    else if (dia == DayOfWeek.Sunday)
                        dgv.Rows[rowIndex].Cells[d].Style.BackColor = Color.Orange;
                }


            }
            string nombreMes = new DateTime(año, mes, 1).ToString("MMMM");
            string texto = $"{nombreMes} de {año}";
            lblfecha.Text = texto;
        }

        private void btnenfermero_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtagregarEnfermero.Text))
            {
                MessageBox.Show("Por favor complete el campo");
                return;
            }

            int mesInicio = cbMes.SelectedIndex + 1; // mes seleccionado
            int anio = DateTime.Now.Year;

            conexionRegistroMensual conexion = new conexionRegistroMensual();

            // Recorremos desde el mes seleccionado hasta diciembre 
            for (int mes = mesInicio; mes <= 12; mes++)
            {
                DateTime fecha = new DateTime(anio, mes, 1);
                string nombreTabla = ObtenerNombreTablaDesdeFecha(fecha);

                conexion.insertarEnfermero(txtagregarEnfermero.Text, nombreTabla);
            }

            cargar();


        }

        private void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoFormulario) return;

            conexionRegistroMensual conexion = new conexionRegistroMensual();

            if (cbMes.SelectedItem != null)
            {
                mesActual = int.Parse(cbMes.SelectedItem.ToString());
                añoActual = CalcularAñoSegunMesSeleccionado(mesActual);
            }

            // ✅ 1. Crear la tabla primero
            conexion.CrearTablaMensualSiNoExiste(mesActual, añoActual);

            // ✅ 2. Verificar si hay registros
            if (!conexion.ExistenRegistrosParaMes(mesActual, añoActual))
            {
                string tablaOrigen = "registro_octubre_2025";

                // ✅ 1. Generar registros base desde octubre
                List<RegistroMensual> registrosBase = conexion.GenerarRegistrosBaseDesdeTablaMensual(tablaOrigen);
                Console.WriteLine($"Se generaron {registrosBase.Count} registros desde octubre");

                // ✅ 2. Insertar en la tabla de noviembre
                conexion.InsertarRegistrosBaseEnTablaMensual(registrosBase, mesActual, añoActual);
            }

            // ✅ 3. Cargar desde SQL lo que realmente hay en noviembre
            List<RegistroMensual> registros = conexion.obtenerRegistro(mesActual, añoActual);
            CrearGrillaDesdeRegistros(dgvPlanilla, registros, mesActual, añoActual);



        }
        public void CrearColumnasDinamicas(DataGridView dgv, int mes, int año)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("IdEmpleado", "ID");
            dgv.Columns.Add("NombreCompleto", "Empleado");

            int dias = DateTime.DaysInMonth(año, mes);
            for (int i = 1; i <= dias; i++)
            {
                dgv.Columns.Add($"Dia{i}", $"Día {i}");
            }
        }

        public static string NombreMes(int mes)// es solamente para pasar el mes a palabras
        {
            string[] nombres = {
        "enero", "febrero", "marzo", "abril", "mayo", "junio",
        "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
    };

            return nombres[mes - 1];
        }



        public int CalcularAñoSegunMesSeleccionado(int mesSeleccionado)
        {
            if (DateTime.Now.Month == 12 && mesSeleccionado == 1)
                return DateTime.Now.Year + 1;
            else
                return DateTime.Now.Year;
        }

        public List<diaVisual> generarDiasRealesDelMes(int mes, int año)
        {
            List <diaVisual> lista = new List<diaVisual>();
            int diaMes = DateTime.DaysInMonth(año, mes);
             for (int i = 1; i <= diaMes; i++)
            { 
                DateTime fecha = new DateTime(año, mes, i);
                string fechaActualizada= fecha.ToString("ddd d/M", new CultureInfo("es-ES"));
                lista.Add(new diaVisual(fecha,fechaActualizada));//devuelve la fecha en palabras
                
            }
            return lista;
        }


        private void dgvPlanilla_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;

            List<diaVisual> diasVisuales = generarDiasRealesDelMes(mesActual, añoActual);
            int indice = e.ColumnIndex - 2;

            if (indice >= 0 && indice < diasVisuales.Count)
            {
                var fecha = diasVisuales[indice].Fecha;
                Color fondo = Color.White;

                if (fecha.DayOfWeek == DayOfWeek.Saturday)
                    fondo = Color.Yellow;
                else if (fecha.DayOfWeek == DayOfWeek.Sunday)
                    fondo = Color.Orange;

                // Pintar fondo
                using (SolidBrush brush = new SolidBrush(fondo))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                // Pintar bordes
                using (Pen pen = new Pen(dgvPlanilla.GridColor))
                {
                    e.Graphics.DrawRectangle(pen, e.CellBounds);
                }

                // Pintar texto si no es nulo
                if (e.Value != null)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.Value.ToString(),
                        e.CellStyle.Font,
                        e.CellBounds,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                }

                e.Handled = true;
            }


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int mes = cbMes.SelectedIndex + 1;
            int anio = DateTime.Now.Year;
            DateTime fecha = new DateTime(anio,mes, 1);
            string nombreTabla = ObtenerNombreTablaDesdeFecha(fecha);

            conexionRegistroMensual registro = new conexionRegistroMensual();


            int filaSeleccionada = dgvPlanilla.CurrentCell.RowIndex;
            var celdaId = dgvPlanilla.Rows[filaSeleccionada].Cells["id_persona"];
            int idSeleccionado;
            bool conversionOk = int.TryParse(celdaId.Value.ToString(), out idSeleccionado);

            if (!conversionOk)
            {
                MessageBox.Show($"El valor de ID no es un número válido: {celdaId.Value}", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
           }
          
            DialogResult mensaje = MessageBox.Show("Seguro que quieres eliminar?", "Confirma elininacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (mensaje != DialogResult.Yes)
                return; 
            registro.eliminar(idSeleccionado,nombreTabla);
            cargar();
            

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int mes = cbMes.SelectedIndex + 1;
            int anio = DateTime.Now.Year;
            DateTime fecha = new DateTime(anio,mes, 1); 
            string nombreTabla = ObtenerNombreTablaDesdeFecha(fecha);


            conexionRegistroMensual registro = new conexionRegistroMensual();
            int filaSeleccionada = dgvPlanilla.CurrentCell.RowIndex;
           


            var celdaId = dgvPlanilla.Rows[filaSeleccionada].Cells["IdEmpleado"];
            int id = Convert.ToInt32(dgvPlanilla.Rows[filaSeleccionada].Cells["id_persona"].Value);
            string Seleccionado = celdaId.Value.ToString();
            
            modificarEnfermero modificacion = new modificarEnfermero(Seleccionado,id,nombreTabla);
            modificacion.ShowDialog();
            cargar();
        }
        public static string ObtenerNombreTablaDesdeFecha(DateTime fecha)
        {
            string mes = fecha.ToString("MMMM", new CultureInfo("es-ES")).ToLower(); 
            return $"registro_{mes}_{fecha.Year}";
        }



    }






}
