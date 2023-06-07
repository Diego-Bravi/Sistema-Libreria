using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;


namespace Libreria
{
    internal class clsCliente
    {

        private OleDbConnection conexion = new OleDbConnection();
        private OleDbCommand comando = new OleDbCommand();
        private OleDbDataAdapter adaptador = new OleDbDataAdapter();

        private string CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = Libreria.accdb";
        private string Tabla = "Clientes";


        public Int32 idCli;
        public String Nom;
        public String Dom;
        public String Telef;
        public Decimal Deu;

        public Decimal Deuda
        {

            get { return Deu; }
            set { Deu = value; }
        }

        private Decimal TotDeuda;


        public Decimal TotalDeuda
        {
            get { return TotDeuda; }

        }

        public Int32 IdCliente
        {
            get { return idCli; }
            set { idCli = value; }
        }

        public String Nombre
        {
            get { return Nom; }
            set { Nom = value; }
        }

        public String Domicilio
        {
            get { return Dom; }
            set { Dom = value; }
        }

        public String Telefono
        {
            get { return Telef; }
            set { Telef = value; }
        }
 

        public void Agregar()
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;
                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);

                DataTable tabla = DS.Tables[Tabla];
                DataRow fila = tabla.NewRow();

                fila["idCliente"] = idCli + 1;
                fila["NombreCliente"] = Nom;
                fila["Domicilio"] = Dom;
                fila["Telefono"] = Telef;
                fila["Deuda"] = 0;


                tabla.Rows.Add(fila);
                OleDbCommandBuilder ConciliaCambios = new OleDbCommandBuilder(adaptador);
                adaptador.Update(DS, Tabla);
                conexion.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ImprimirClientes()
        {

            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);

                StreamWriter AD = new StreamWriter("ReporteClientes.csv", false, Encoding.UTF8);
                AD.WriteLine("Listado de Clientes");
                AD.WriteLine("Nombre; Domicilio; Telefono");

                if (DS.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow x in DS.Tables[Tabla].Rows) 
                    {
                        AD.Write(x["Nombre"]);
                        AD.Write(";");
                        AD.Write(x["Domicilio"]);
                        AD.Write(";");
                        AD.WriteLine(x["Telefono"]);
                    }
                }
                conexion.Close();
                AD.Close();

            }
            catch(Exception e)  
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        public void ListarClientes(DataGridView Grilla)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);

                TotDeuda = 0;


                Grilla.Rows.Clear();



                if (DS.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow x in DS.Tables[Tabla].Rows)
                    {

                        Grilla.Rows.Add(x["IdCliente"],x["NombreCliente"], x["Domicilio"], x["Deuda"]);

                        TotDeuda = TotDeuda + Convert.ToDecimal(x["Deuda"]);


                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Listar(ComboBox Combo)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);

                Combo.DataSource = DS.Tables[Tabla];
                Combo.DisplayMember = "NombreCliente";
                Combo.ValueMember = "IdCliente";


                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Eliminar(Int32 IdCliente)
        {
            try
            {
                String sql = "DELETE * FROM Clientes WHERE IdCliente = " + IdCliente.ToString();
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

        public void Buscar(Int32 IdCliente)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);



                if (DS.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow x in DS.Tables[Tabla].Rows)
                    {
                        if (Convert.ToInt32(x["IdCliente"]) == IdCliente)
                        {
                            idCli = Convert.ToInt32(x["IdCliente"]);
                            Deu = Convert.ToDecimal(x["Deuda"]);
                        }

                    }
                }

                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Modificar(Int32 IdCliente)
        {
            try
            {
                String sql = "";
                sql = "UPDATE Clientes SET DEUDA = " + Deu.ToString() + " WHERE IdCliente = " + IdCliente.ToString();
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
