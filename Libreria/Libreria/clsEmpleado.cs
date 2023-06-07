using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Security.Cryptography;

namespace Libreria
{
    internal class clsEmpleado
    {

        private OleDbConnection conexion = new OleDbConnection();
        private OleDbCommand comando = new OleDbCommand();
        private OleDbDataAdapter adaptador = new OleDbDataAdapter();

        private string CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = Libreria.accdb";
        private string Tabla = "Empleados";

        public Int32 idEmpl;
        public String Nom;
        public String Dom;
        public String Telef;
        public String Carg;

        public String Cargo
        {

            get { return Carg; }
            set { Carg = value; }
        }

        public Int32 IdEmpleado
        {
            get { return idEmpl; }
            set { idEmpl = value; }
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
                Combo.DisplayMember = "NombreEmpleado";
                Combo.ValueMember = "IdEmpleado";


                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ListarEmpleados(DataGridView Grilla)
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


                Grilla.Rows.Clear();
                


                if (DS.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow x in DS.Tables[Tabla].Rows)
                    {

                        Grilla.Rows.Add(x["IdEmpleado"], x["NombreEmpleado"], x["Domicilio"], x["Telefono"], x["Cargo"]);

                        
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void Eliminar(Int32 IdEmpleado)
        {
            try
            {
                String sql = "DELETE * FROM Empleados WHERE IdEmpleado = " + IdEmpleado.ToString();
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

                fila["IdEmpleado"] = idEmpl + 1;
                fila["NombreEmpleado"] = Nom;
                fila["Domicilio"] = Dom;
                fila["Telefono"] = Telef;
                fila["Cargo"] = Carg;


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

        public String Buscar(Int32 IdEmpleado)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();

                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                OleDbDataReader DR = comando.ExecuteReader();
                String Resultado = "";

                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        if (DR.GetInt32(0) == IdEmpleado)
                        {
                            Resultado = DR.GetString(1);
                        }
                    }

                }
                conexion.Close();
                return Resultado;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

    }
}
