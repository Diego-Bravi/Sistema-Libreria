using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libreria
{
    public partial class frmAgregarCliente : Form
    {
        public frmAgregarCliente()
        {
            InitializeComponent();
        }

        private void cmdCargar_Click(object sender, EventArgs e)
        {
            clsCliente cli = new clsCliente();

            if(txtDomicilio.Text != "" && txtNombre.Text != "" && txtTelefono.Text != "")
            {
                cli.Nombre = txtNombre.Text;
                cli.Domicilio = txtDomicilio.Text;
                cli.Telefono = txtTelefono.Text;
               

                cli.Agregar();
                MessageBox.Show("Datos Agregados");
                txtNombre.Text = "";
                txtDomicilio.Text = "";
                txtTelefono.Text = "";
                
            }
            else
            {
                MessageBox.Show("Completar todos los Datos!");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
