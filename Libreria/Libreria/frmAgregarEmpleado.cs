using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Libreria
{
    public partial class frmAgregarEmpleado : Form
    {
        public frmAgregarEmpleado()
        {
            InitializeComponent();
        }
        clsEmpleado emp = new clsEmpleado();
        private void cmdCargar_Click(object sender, EventArgs e)
        {
            

            if (txtDomicilio.Text != "" && txtNombre.Text != "" && txtTelefono.Text != "")
            {
                emp.Nombre = txtNombre.Text;
                emp.Domicilio = txtDomicilio.Text;
                emp.Telefono = txtTelefono.Text;
                emp.Cargo = cmbCargo.SelectedItem.ToString();

                emp.Agregar();
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

        private void frmAgregarEmpleado_Load(object sender, EventArgs e)
        {
            cmbCargo.SelectedIndex = 0;
        }

        private void cmbCargo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
