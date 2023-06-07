using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;



namespace Libreria
{
    public partial class frmGrafico : Form
    {

        private string conexion = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = Libreria.accdb";

        private PrintDocument printDocument = new PrintDocument();

        public frmGrafico()
        {
            InitializeComponent();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPageHandler);

        }

        private void frmGrafico_Load(object sender, EventArgs e)
        {

        }

       

        private void cmdImprimir_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
            MessageBox.Show("Grafico Impreso");
        }

        private void cmdCalcular_Click(object sender, EventArgs e)
        {


            DateTime fechaInicio = dtpFechaInicio.Value;
            DateTime fechaFin = dtpFechaFin.Value;

            using (OleDbConnection connection = new OleDbConnection(conexion))
            {
                connection.Open();

                
                string Query = $"SELECT Fecha, COUNT(*) AS Cantidad FROM Ventas WHERE Fecha BETWEEN #{fechaInicio.ToString("MM/dd/yyyy")}# AND #{fechaFin.ToString("MM/dd/yyyy")}# GROUP BY Fecha";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(Query, connection);
                DataTable DataVentas = new DataTable();
                dataAdapter.Fill(DataVentas);

                
                int width = prtGrafico.Width;
                int height = prtGrafico.Height;
                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.White);

                int maxValue = 0;            
                int barWidth = width / (DataVentas.Rows.Count * 2);
                int x = 20;
                int y = 20;

                foreach (DataRow row in DataVentas.Rows)
                {
                    DateTime Fecha = Convert.ToDateTime(row["Fecha"]);
                    int Cantidad = Convert.ToInt32(row["Cantidad"]);

                    if (Cantidad > maxValue)
                    {
                        maxValue = Cantidad;
                    }

                    int barHeight = (int)(((double)Cantidad / maxValue) * (height - 40));

                

                    graphics.FillRectangle(Brushes.Blue, x, y + height - barHeight, barWidth, barHeight);

                    string label = $"Fecha :{Fecha.ToShortDateString()}\n Cantidad Vendida:{Cantidad}";
                    SizeF labelSize = graphics.MeasureString(label, Font);
                    PointF labelLocation = new PointF(x, y + height - barHeight - labelSize.Height - 5);
                    graphics.DrawString(label, Font, Brushes.Black, labelLocation);

                    x += barWidth * 2;
                }

               
                prtGrafico.Image = bitmap;

                
                lblFecha.Text = $"Fechas: {fechaInicio.ToShortDateString()} - {fechaFin.ToShortDateString()}";

                connection.Close();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Bitmap chartBitmap = (Bitmap)prtGrafico.Image;
            string fechas = lblFecha.Text;

            
            Rectangle chartRect = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height - 50);
            graphics.DrawImage(chartBitmap, chartRect);

            Font datesFont = new Font("Arial", 12, FontStyle.Bold);
            Rectangle datesRect = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Bottom - 50, e.MarginBounds.Width, 50);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            graphics.DrawString(fechas, datesFont, Brushes.Black, datesRect, format);
        }
    }
}
