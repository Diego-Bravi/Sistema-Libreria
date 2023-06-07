using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Libreria
{
    public partial class frmGraficoVentas : Form
    {

        private const string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = Libreria.accdb";

        public frmGraficoVentas()
        {
            InitializeComponent();
        }

        private void cmdCalcular_Click(object sender, EventArgs e)
        {


            chart1.Series.Clear(); 

            DateTime fechaInicio = dtpFechaInicio.Value;
            DateTime fechaFin = dtpFechaFin.Value;

            
            Series serieVentas = new Series("Ventas");
            serieVentas.ChartType = SeriesChartType.Column; 

          
            DateTime fechaMin, fechaMax;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string queryMinFecha = "SELECT MIN(Fecha) FROM Ventas";
                string queryMaxFecha = "SELECT MAX(Fecha) FROM Ventas";
                OleDbCommand commandMinFecha = new OleDbCommand(queryMinFecha, connection);
                OleDbCommand commandMaxFecha = new OleDbCommand(queryMaxFecha, connection);
                fechaMin = Convert.ToDateTime(commandMinFecha.ExecuteScalar());
                fechaMax = Convert.ToDateTime(commandMaxFecha.ExecuteScalar());
            }

           
            if (fechaInicio < fechaMin)
                fechaInicio = fechaMin;
            if (fechaFin > fechaMax)
                fechaFin = fechaMax;

          
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Fecha, SUM(Cantidad) AS TotalVentas FROM Ventas GROUP BY Fecha";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime fecha = Convert.ToDateTime(reader["Fecha"]);
                    int totalVentas = Convert.ToInt32(reader["TotalVentas"]);

                    if (fecha >= fechaInicio && fecha <= fechaFin)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisLabel = fecha.ToString("dd/MM/yyyy"); 
                        dataPoint.YValues = new double[] { totalVentas };
                        serieVentas.Points.Add(dataPoint);
                    }
                }

                reader.Close();
            }

            chart1.Series.Add(serieVentas); 

           
            lblFecha.Text = $"Fechas extremas: {fechaMin.ToString("dd/MM/yyyy")} - {fechaMax.ToString("dd/MM/yyyy")}";
        }

        private void cmdImprimir_Click(object sender, EventArgs e)
        {

            PrintDocument pd = new PrintDocument();


            pd.PrintPage += Pd_PrintPage;


            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }

            MessageBox.Show("Grafico Impreso");
        }
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            chart1.Printing.PrintPaint(e.Graphics, new Rectangle(10, 10, e.PageBounds.Width - 20, e.PageBounds.Height - 20));
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

        }
    }
}



