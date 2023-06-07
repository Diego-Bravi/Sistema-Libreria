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
    public partial class frmListarConsumo : Form
    {
        public frmListarConsumo()
        {
            InitializeComponent();

        }
        clsCliente Cli = new clsCliente();
        clsVenta venta = new clsVenta();

        private void frmListarConsumo_Load(object sender, EventArgs e)
        {
            


            Cli.Listar(cmbNombre);
        }

        private void cmdListar_Click(object sender, EventArgs e)
        {

            venta.idCliente= Convert.ToInt32(cmbNombre.SelectedValue);
            venta.ListarConsumoClientes(dgvDetalle);
            lblTotal.Text = venta.TotalDeuda.ToString("0.00");

            cmdImprimir.Enabled = true;
          
        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            prtVentana.ShowDialog();
            prtDocument.PrinterSettings = prtVentana.PrinterSettings;
            prtDocument.Print();
            MessageBox.Show("Reporte Impreso");
        }

        private void cmdEliminar_Click(object sender, EventArgs e)
        {
           
        }

        private void prtDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //clsVenta ven = new clsVenta();

            venta.ImprimirConsumoClientes(e);
        }
    }
}
