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
    public partial class frmCargarVenta : Form
    {
        public frmCargarVenta()
        {
            InitializeComponent();
        }
         
        clsVenta x = new clsVenta();
        private void button1_Click(object sender, EventArgs e)
        {
            if(txtCantidad.Text != "")
            {
              
                x.AgregarVenta(Convert.ToInt32(cmbNombrePro.SelectedValue), Convert.ToInt32(cmbNombreCli.SelectedValue), Convert.ToInt32(cmbNombreVen.SelectedValue), Convert.ToDateTime(dtpFecha.Value.Date), Convert.ToInt32(txtCantidad.Text));
                MessageBox.Show("Datos Cargados");
                txtCantidad.Text = "";
                
            }
            else
            {
                MessageBox.Show("Completar Cantidad!");
            }
        }

        private void frmCargarVenta_Load(object sender, EventArgs e)
        {

            clsCliente Cli = new clsCliente();
            clsProducto Pro = new clsProducto();
            clsEmpleado Emple = new clsEmpleado();


            Cli.Listar(cmbNombreCli);
            Emple.Listar(cmbNombreVen);
            Pro.Listar(cmbNombrePro);

        }
    }
}
