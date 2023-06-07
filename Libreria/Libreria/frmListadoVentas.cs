using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libreria
{
    public partial class frmListadoVentas : Form
    {
        public frmListadoVentas()
        {
            InitializeComponent();
        }

        clsVenta venta = new clsVenta();
        private void cmdListar_Click(object sender, EventArgs e)
        {
           
            venta.ListarVentas(dgvDetalle);
            lblTotal.Text = venta.TotalDeuda.ToString("0.00");
            cmdImprimir.Enabled = true;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            prtVentana.ShowDialog();
            prtDocument.PrinterSettings = prtVentana.PrinterSettings;
            prtDocument.Print();
            MessageBox.Show("Reporte Impreso");
        }

        private void prtDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            clsVenta ven = new clsVenta();

            ven.ImprimirVentas(e);
        }
    }
}
