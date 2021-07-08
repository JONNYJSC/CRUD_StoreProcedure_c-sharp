using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class FormEmpleado : System.Windows.Forms.Form
    {
        string conString = "Data Source=JONNYJSC-DELL; Initial Catalog=db_Empleado; User Id=sa; Password=admin123;";

        SqlConnection sqlCon;
        SqlCommand sqlCmd;
        string EmpleadoId = "";
        public FormEmpleado()
        {
            InitializeComponent();
            sqlCon = new SqlConnection(conString);
            sqlCon.Open();
        }
        private DataTable MostrarRegistrosEmp(string empId)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            DataTable dtData = new DataTable();
            sqlCmd = new SqlCommand("spEmpleado", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ActionType", "MostrarRegistro");
            sqlCmd.Parameters.AddWithValue("@EmpleadoId", empId);
            SqlDataAdapter sqlSda = new SqlDataAdapter(sqlCmd);
            sqlSda.Fill(dtData);
            return dtData;
        }
        private DataTable MostrarDetallesEmp()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            DataTable dtData = new DataTable();
            sqlCmd = new SqlCommand("spEmpleado", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ActionType", "MostrarDato");
            SqlDataAdapter sqlSda = new SqlDataAdapter(sqlCmd);
            sqlSda.Fill(dtData);
            return dtData;
        }
        private void FormEmpleado_Load(object sender, EventArgs e)
        {
            // Para ocultar la columna 1 del DataGridView
            dgvEmpleado.Columns[0].Visible = false;
            dgvEmpleado.DataSource = MostrarDetallesEmp();            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingresar nombre del empleado !!!");
                txtNombre.Select();
            }
            else if (string.IsNullOrWhiteSpace(txtCiudad.Text))
            {
                MessageBox.Show("Ingresar ciudad del empleado !!!");
                txtCiudad.Select();
            }
            else
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    DataTable dtData = new DataTable();
                    sqlCmd = new SqlCommand("spEmpleado", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@ActionType", "GuardarDato");
                    sqlCmd.Parameters.AddWithValue("@EmpleadoId", EmpleadoId);
                    sqlCmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    sqlCmd.Parameters.AddWithValue("@Ciudad", txtCiudad.Text);
                    int numRes = sqlCmd.ExecuteNonQuery();
                    if (numRes > 0)
                    {
                        MessageBox.Show("Registro guardado con éxito !!!");
                        Limpiar();
                    }
                    else
                        MessageBox.Show("Inténtalo de nuevo !!!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:- " + ex.Message);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EmpleadoId))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    DataTable dtData = new DataTable();
                    sqlCmd = new SqlCommand("spEmpleado", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@ActionType", "EliminarDato");
                    sqlCmd.Parameters.AddWithValue("@EmpleadoId", EmpleadoId);
                    int numRes = sqlCmd.ExecuteNonQuery();
                    if (numRes > 0)
                    {
                        MessageBox.Show("Registro eliminado con éxito !!!");
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show("Inténtalo de nuevo !!!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:- " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro!!!");
            }
        }

        private void Limpiar()
        {
            btnGuardar.Text = "Guardar";
            txtNombre.Text = "";
            txtCiudad.Text = "";
            EmpleadoId = "";
            dgvEmpleado.AutoGenerateColumns = false;
            dgvEmpleado.DataSource = MostrarDetallesEmp();
        }

        private void dgvEmpleado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnGuardar.Text = "Actualizar";
                EmpleadoId = dgvEmpleado.Rows[e.RowIndex].Cells[0].Value.ToString();
                DataTable dtData = MostrarRegistrosEmp(EmpleadoId);
                if (dtData.Rows.Count > 0)
                {
                    EmpleadoId = dtData.Rows[0][0].ToString();
                    txtNombre.Text = dtData.Rows[0][1].ToString();
                    txtCiudad.Text = dtData.Rows[0][2].ToString();
                }
                else
                {
                    Limpiar();
                }
            }
        }
    }
}
