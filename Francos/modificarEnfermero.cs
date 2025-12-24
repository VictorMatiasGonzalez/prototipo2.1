using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;

namespace Francos
{
    public partial class modificarEnfermero : Form
    {
        public modificarEnfermero()
        {
            InitializeComponent();
        }
        private int idPersona;
        private string nombreTabla;


        public modificarEnfermero(string nombreActual, int id, string tabla)
        {
            InitializeComponent();
            txtNombreyApellido.Text = nombreActual;
            idPersona = id;
            nombreTabla = tabla;
        }



        

        private void btnAceptarMod_Click(object sender, EventArgs e)
        {
            conexionRegistroMensual conexion =  new conexionRegistroMensual();
            conexion.updateEnfermero(txtNombreyApellido.Text,idPersona, nombreTabla);
            if (txtNombreyApellido.Text == "")
            {
                MessageBox.Show("Por favor escriba el nombre del enfermero/a");
                    return;
            }
            

        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
