using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing.Printing;
using System.Drawing;

namespace Libreria
{
    internal class clsVenta
    {

        private OleDbConnection conexion = new OleDbConnection();
        private OleDbCommand comando = new OleDbCommand();
        private OleDbDataAdapter adaptador = new OleDbDataAdapter();

        private string CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = Libreria.accdb";
        


        private Int32 idEmpl;
        private Int32 idVen;
        private Int32 idProdu;
        private Int32 idCli;
        private DateTime Fe;
        private Int32 Cant;
        private clsCliente cliente = new clsCliente();
        private clsProducto producto = new clsProducto();
        private clsEmpleado empleado = new clsEmpleado();



        private Double TotDeuda;


        public Double TotalDeuda
        {
            get { return TotDeuda; }

        }


        public Int32 Cantidad
        {
            get { return Cant; }
            set { Cant = value; }
        }

        public Int32 idVenta
        {
            get { return idVen; }
            set { idVen = value; }
        }

        public Int32 idEmpleado
        {
            get { return idEmpl; }
            set { idEmpl = value; }
        }

        public Int32 idProducto
        {
            get { return idProdu; }
            set { idProdu = value; }
        }

        public Int32 idCliente
        {
            get { return idCli; }
            set { idCli = value; }
        }

        public DateTime Fecha
        {
            get { return Fe; }
            set { Fe = value; }
        }

        public void AgregarVenta(int idProducto, int idCliente, int idEmpleado, DateTime fecha, int cantidad)
        {
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();

                    using (OleDbCommand comando = new OleDbCommand())
                    {
                        comando.Connection = conexion;


                        comando.CommandText = "INSERT INTO ventas ( IdProducto, IdCliente, IdEmpleado, Fecha, Cantidad) " +
                            "VALUES ( @IdProducto, @IdCliente, @IdEmpleado, @Fecha, @Cantidad)";

                        
                        comando.Parameters.AddWithValue("@IdProducto", idProducto);
                        comando.Parameters.AddWithValue("@IdCliente", idCliente);
                        comando.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                        comando.Parameters.AddWithValue("@Fecha", fecha);
                        comando.Parameters.AddWithValue("@Cantidad", cantidad);

                        comando.ExecuteNonQuery();
                    }

                    double precioProducto = ObtenerPrecioProducto(idProducto);
                    double deuda = cantidad * precioProducto;

                    ActualizarDeudaCliente(idCliente, deuda);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private double ObtenerPrecioProducto(int idProducto)
        {
            double precio = 0;

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();

                    using (OleDbCommand comando = new OleDbCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = "SELECT ValorProducto FROM productos WHERE IdProducto = @IdProducto";
                        comando.Parameters.AddWithValue("@IdProducto", idProducto);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null && resultado != DBNull.Value)
                        {
                            precio = Convert.ToDouble(resultado);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return precio;
        }



        private void ActualizarDeudaCliente(int idCliente, double deuda)
        {
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();

                    using (OleDbCommand comando = new OleDbCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = "UPDATE clientes SET Deuda = @Deuda WHERE IdCliente = @IdCliente";
                        comando.Parameters.AddWithValue("@Deuda", deuda);
                        comando.Parameters.AddWithValue("@IdCliente", idCliente);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }




        public void ListarVentasPorVendedor(DataGridView Grilla, int IdEmpleado, DateTime fechaSeleccionada)
        {
            try
            {
                string connectionString = CadenaConexion;
                string query = "SELECT * FROM (Ventas INNER JOIN Productos ON Ventas.IdProducto = Productos.IdProducto) INNER JOIN Empleados ON Ventas.IdEmpleado = Empleados.IdEmpleado WHERE Ventas.IdEmpleado = @IdEmpleado AND Ventas.Fecha >= @FechaInicio AND Ventas.Fecha < @FechaFin";

                DateTime fechaInicio = fechaSeleccionada.Date;
                DateTime fechaFin = fechaInicio.AddDays(1);

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@IdEmpleado", IdEmpleado);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    OleDbDataReader reader = command.ExecuteReader();

                    Grilla.Rows.Clear();

                    while (reader.Read())
                    {
                        string IdVentas = reader["IdVenta"].ToString();
                        int cantidad = int.Parse(reader["Cantidad"].ToString());
                        double precio = double.Parse(reader["ValorProducto"].ToString());
                        double importeTotal = cantidad * precio;
                        string nombreProducto = reader["NombreProducto"].ToString();

                        Grilla.Rows.Add(IdVentas, nombreProducto, cantidad, precio, importeTotal);

                        TotDeuda += importeTotal;
                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ListarConsumoClientes(DataGridView Grilla)
        {
            try
            {

                string connectionString = CadenaConexion;
                string query = "SELECT * FROM Ventas INNER JOIN Productos ON Ventas.IdProducto = Productos.IdProducto WHERE Ventas.IdCliente = @IdCliente";




                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@IdCliente", idCli);
                    OleDbDataReader reader = command.ExecuteReader();
                    

                    Grilla.Rows.Clear();



                    while (reader.Read())
                    {

                        string IdVentas = reader["IdVenta"].ToString();
                        string nombreProducto = reader["NombreProducto"].ToString();
                        int cantidad = int.Parse(reader["Cantidad"].ToString());
                        double precio = double.Parse(reader["ValorProducto"].ToString());
                        double importeTotal = cantidad * precio;

                        Grilla.Rows.Add(IdVentas,nombreProducto, cantidad, precio, importeTotal);

                        TotDeuda += importeTotal;


                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ImprimirConsumoClientes(PrintPageEventArgs Reporte)
        {
            Font fuenteTitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fuenteDatos = new Font("Arial", 12);
            SolidBrush pincel = new SolidBrush(Color.Black);

            int xTitulo = 50;
            int y = 50;

            
            Reporte.Graphics.DrawString("Reporte de Consumo del Cliente", new Font("Arial", 16, FontStyle.Bold), pincel, xTitulo, y);
          

            try
            {
                string connectionString = CadenaConexion;
                string query = "SELECT * FROM Ventas INNER JOIN Productos ON Ventas.IdProducto = Productos.IdProducto WHERE Ventas.IdCliente = @IdCliente";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string nombreCliente = string.Empty;
                    string queryCliente = "SELECT NombreCliente FROM Clientes WHERE IdCliente = @IdCliente";
                    OleDbCommand commandCliente = new OleDbCommand(queryCliente, connection);
                    commandCliente.Parameters.AddWithValue("@IdCliente", idCli);
                    nombreCliente = commandCliente.ExecuteScalar().ToString();

                    y += 50;
                    Reporte.Graphics.DrawString("Cliente: " + nombreCliente, fuenteDatos, pincel, xTitulo, y);
                    y += 50;

                    string titulos = "  ID -   Producto  -  Cantidad  -  Precio  -  Total";
                    Reporte.Graphics.DrawString(titulos, fuenteTitulo, pincel, xTitulo, y);
                    y += 30;

                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@IdCliente", idCli);
                    OleDbDataReader reader = command.ExecuteReader();

                    double totalVentas = 0;



                    while (reader.Read())
                    {
                        
                        string IdVentas = reader["IdVenta"].ToString();
                        string nombreProducto = reader["NombreProducto"].ToString();
                        int cantidad = int.Parse(reader["Cantidad"].ToString());
                        double precio = double.Parse(reader["ValorProducto"].ToString());
                        double importeTotal = cantidad * precio;

                        totalVentas += importeTotal;

                        string linea = $"{IdVentas} - {nombreProducto} - {cantidad} - {precio} - {importeTotal}";

                        Reporte.Graphics.DrawString(IdVentas, fuenteDatos, pincel, 70 , y);
                        Reporte.Graphics.DrawString(nombreProducto, fuenteDatos, pincel, 120, y);
                        Reporte.Graphics.DrawString(cantidad.ToString(), fuenteDatos, pincel, 250, y);
                        Reporte.Graphics.DrawString(precio.ToString(), fuenteDatos, pincel, 350, y);
                        Reporte.Graphics.DrawString(importeTotal.ToString(), fuenteDatos, pincel, 430 , y);

                        y += 30;
                    }

                    y += 50;
                    string pie = $"Consumo Total: {totalVentas.ToString()}"; 
                    Reporte.Graphics.DrawString(pie, fuenteDatos, pincel, xTitulo, y);


                    reader.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

          
        }

        public void ListarVentas(DataGridView Grilla)
        {

            try
            {

                string connectionString = CadenaConexion;
                string query = "SELECT *  from ((Ventas left JOIN Clientes  ON Ventas.IdCliente = Clientes.IdCliente) " +
                   "left JOIN Productos  ON Ventas.IdProducto = Productos.IdProducto) " +
                   "left join Empleados on Empleados.IdEmpleado = Ventas.IdEmpleado";


                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataReader reader = command.ExecuteReader();

                    Grilla.Rows.Clear();



                    while (reader.Read())
                    {
                        string IdVentas = reader["IdVenta"].ToString();
                        string NombreCliente = reader["NombreCliente"].ToString();
                        string nombreProducto = reader["NombreProducto"].ToString();
                        string nombreEmpleado = reader["NombreEmpleado"].ToString();
                        string fecha = reader["Fecha"].ToString();
                        int cantidad = int.Parse(reader["Cantidad"].ToString());
                        double precio = double.Parse(reader["ValorProducto"].ToString());
                        double importeTotal = cantidad * precio;
                        Grilla.Rows.Add(IdVentas, fecha, nombreProducto, NombreCliente, cantidad,precio,importeTotal, nombreEmpleado);

                        TotDeuda += importeTotal;


                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }

        public void ImprimirVentas(PrintPageEventArgs Reporte)
        {
            Font fuenteTitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fuenteDatos = new Font("Arial", 12);
            SolidBrush pincel = new SolidBrush(Color.Black);

            int xTitulo = 50;
            int y = 50;

            
            Reporte.Graphics.DrawString("Reporte de Ventas", new Font("Arial", 16, FontStyle.Bold), pincel, xTitulo, y);
            y += 50;

            
            string titulos = "ID -   Fecha   -   Producto  -  Cliente -  Cantidad  - Precio -   Total   -    Empleado";
            Reporte.Graphics.DrawString(titulos, fuenteTitulo, pincel, xTitulo, y);
            y += 30;

            try
            {
                string connectionString = CadenaConexion;
                string query = "SELECT *  from ((Ventas left JOIN Clientes ON Ventas.IdCliente = Clientes.IdCliente) " +
                               "left JOIN Productos ON Ventas.IdProducto = Productos.IdProducto) " +
                               "left join Empleados on Empleados.IdEmpleado = Ventas.IdEmpleado";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataReader reader = command.ExecuteReader();

                    double totalVentas = 0;

                    
                    while (reader.Read())
                    {
                        string idVentas = reader["IdVenta"].ToString();
                        DateTime fecha = Convert.ToDateTime(reader["Fecha"]);
                        string nombreProducto = reader["NombreProducto"].ToString();
                        string nombreCliente = reader["NombreCliente"].ToString();
                        int cantidad = int.Parse(reader["Cantidad"].ToString());
                        double precio = double.Parse(reader["ValorProducto"].ToString());
                        double importeTotal = cantidad * precio;
                        string nombreEmpleado = reader["NombreEmpleado"].ToString();

                        totalVentas += importeTotal;

                        string linea = $"{idVentas} - {fecha.ToShortDateString()} - {nombreProducto} - {nombreCliente} - {cantidad} - {precio} - {importeTotal} - {nombreEmpleado}";
                        Reporte.Graphics.DrawString(idVentas, fuenteDatos, pincel, 55, y);
                        Reporte.Graphics.DrawString(fecha.ToShortDateString(), fuenteDatos, pincel, 90, y);
                        Reporte.Graphics.DrawString(nombreProducto, fuenteDatos, pincel, 200, y);
                        Reporte.Graphics.DrawString(nombreCliente, fuenteDatos, pincel, 300, y);
                        Reporte.Graphics.DrawString(cantidad.ToString(), fuenteDatos, pincel, 430, y);
                        Reporte.Graphics.DrawString(precio.ToString(), fuenteDatos, pincel,520, y);
                        Reporte.Graphics.DrawString(importeTotal.ToString(), fuenteDatos, pincel,600, y);
                        Reporte.Graphics.DrawString(nombreEmpleado, fuenteDatos, pincel,700, y);

                        y += 30;


                    }

                    y += 50;
                    string pie = $"Total de ventas: {totalVentas.ToString()}"; 
                    Reporte.Graphics.DrawString(pie, fuenteDatos, pincel, xTitulo, y);


                    reader.Close();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

         
       
        }



        public void Eliminar(Int32 IdVenta)
        {
            try
            {
                String sql = "DELETE * FROM Ventas WHERE IdVenta = " + IdVenta.ToString();
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();




                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
