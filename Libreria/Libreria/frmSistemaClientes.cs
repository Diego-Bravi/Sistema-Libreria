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
    public partial class frmSistemaClientes : Form
    {
        public frmSistemaClientes()
        {
            InitializeComponent();
        }

        private void SistemaClientes_Load(object sender, EventArgs e)
        {

        }


        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void agregarNuevoClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAgregarCliente agregarCliente = new frmAgregarCliente();
            
            agregarCliente.Show();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAcercade Acercade = new frmAcercade();

            Acercade.Show();
        }

        private void cargarUnaVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCargarVenta carga = new frmCargarVenta();

            carga.Show();
        }

        private void listadoDeVentasPorVendedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        private void listarLosClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarClientes listarClientes = new frmListarClientes();
            listarClientes.Show();
        }

        private void listadoTotalDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListadoVentas ListadoVentas = new frmListadoVentas();
            ListadoVentas.Show();

        }

        private void consumoDelClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarConsumo consumo = new frmListarConsumo();
            consumo.Show();

        }

        private void cargarDeudaDelClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void agregarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAgregarEmpleado agregarEmpleado = new frmAgregarEmpleado();
            agregarEmpleado.Show();
        }

        private void listarEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarEmpleados listarEmpleados = new frmListarEmpleados();
            listarEmpleados.Show();
        }

        private void listadoDeVentasPorEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListarVentasPorVendedor ListaVendedor = new frmListarVentasPorVendedor();
            ListaVendedor.Show();
        }

        private void graficarEstadisticaVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGraficoVentas Grafico = new frmGraficoVentas();  
            Grafico.Show();
        }

        private void graficarEstadisticaVentaschartingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
        }
    }
}
