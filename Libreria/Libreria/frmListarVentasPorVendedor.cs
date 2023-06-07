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
    public partial class frmListarVentasPorVendedor : Form
    {
        public frmListarVentasPorVendedor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            clsVenta x = new clsVenta();

            int idEmpleado = Convert.ToInt32(cmbNombre.SelectedValue);
            DateTime fechaSeleccionada = dtpFecha.Value;

            x.ListarVentasPorVendedor(dgvVentas, idEmpleado, fechaSeleccionada);
            lblTotal.Text = x.TotalDeuda.ToString("0.00");




        }

        private void frmListarVentasPorVendedor_Load(object sender, EventArgs e)
        {
            clsEmpleado Emple = new clsEmpleado();

            Emple.Listar(cmbNombre);
           
        }
    }
}
