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
    public partial class frmListarClientes : Form
    {
        public frmListarClientes()
        {
            InitializeComponent();
        }

        clsCliente cliente = new clsCliente();

        private void cmdListar_Click(object sender, EventArgs e)
        {
            clsCliente cliente = new clsCliente();
           
            cliente.ListarClientes(dgvCliente);
            lblTotal.Text = cliente.TotalDeuda.ToString("0.00");
            cmdEliminar.Enabled = true;


        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            clsCliente cli = new clsCliente();

            cli.ImprimirClientes();

            MessageBox.Show("Datos Exportados");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "" )
            {
                cliente.Eliminar(Convert.ToInt32(txtCodigo.Text));
                MessageBox.Show("Dato Eliminado Exitosamente");
                cliente.ListarClientes(dgvCliente);
                lblTotal.Text = cliente.TotalDeuda.ToString("0.00");
                txtCodigo.Text = "";
            }
            else
            {
                MessageBox.Show("Comletar con ID Valido");
            }
        }
    }
}
