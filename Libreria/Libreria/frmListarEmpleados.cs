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
    public partial class frmListarEmpleados : Form
    {
        public frmListarEmpleados()
        {
            InitializeComponent();
        }

        private void frmListarEmpleados_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmdListar_Click(object sender, EventArgs e)
        {
            clsEmpleado emp = new clsEmpleado();
           
            emp.ListarEmpleados(dgvCliente);
            cmdEliminar.Enabled = true;

        }

        private void cmdEliminar_Click(object sender, EventArgs e)
        {
            if(txtCodigo.Text != "")
            {
                clsEmpleado emp = new clsEmpleado();

                emp.Eliminar(Convert.ToInt32(txtCodigo.Text));
                MessageBox.Show("Dato Eliminado Exitosamente");
                emp.ListarEmpleados(dgvCliente);
                txtCodigo.Text = "";
            }
            else
            {
                MessageBox.Show("Completar con ID Valido");
            }
        }
    }
}
