using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_GA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conexion = new SqlConnection("Data Source=LOCALHOST\\SQLEXPRESS; database=GRUPO_ARCENSA; Integrated Security=True");
        DB_GA dbGestorEmpleados = new DB_GA();
        private int idEmpleadoSeleccionado = 0;

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                MessageBox.Show("Conectado papá", "GRUPO ARCENSA");
                StartPosition = FormStartPosition.CenterScreen;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM EMPLEADOS", conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DGV.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

       

    

        // Evento para seleccionar una fila del DataGridView y cargar los datos en los campos
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow fila = DGV.Rows[e.RowIndex];

                    idEmpleadoSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
                    txtNombre.Text = fila.Cells["NOMBRE"].Value.ToString();
                    txtApellido.Text = fila.Cells["APELLIDO"].Value.ToString();
                    txtCargo.Text = fila.Cells["CARGO"].Value.ToString();
                    txtSalario.Text = fila.Cells["SALARIO"].Value.ToString();
                    txtCorreo.Text = fila.Cells["CORREO"].Value.ToString();
                    dtIngreso.Value = Convert.ToDateTime(fila.Cells["INGRESO"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar empleado: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtCargo.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios (Nombre, Apellido, Cargo)", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar el salario
                decimal salario;
                if (!decimal.TryParse(txtSalario.Text, out salario))
                {
                    MessageBox.Show("El salario debe ser un número válido", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear objeto empleado
                EMPLEADO nuevoEmpleado = new EMPLEADO()
                {
                    NOMBRE = txtNombre.Text.Trim(),
                    APELLIDO = txtApellido.Text.Trim(),
                    CARGO = txtCargo.Text.Trim(),
                    INGRESO = dtIngreso.Value,
                    SALARIO = salario,
                    CORREO = txtCorreo.Text.Trim()
                };

                // Agregar a la base de datos
                if (dbGestorEmpleados.Agregar(nuevoEmpleado))
                {
                    MessageBox.Show("Empleado agregado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar el empleado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar empleado: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que se haya seleccionado un empleado
                if (idEmpleadoSeleccionado == 0)
                {
                    MessageBox.Show("Por favor, seleccione un empleado del DataGridView para actualizar",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtCargo.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar el salario
                decimal salario;
                if (!decimal.TryParse(txtSalario.Text, out salario))
                {
                    MessageBox.Show("El salario debe ser un número válido", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear objeto empleado con los datos actualizados
                EMPLEADO empleadoActualizado = new EMPLEADO()
                {
                    ID = idEmpleadoSeleccionado,
                    NOMBRE = txtNombre.Text.Trim(),
                    APELLIDO = txtApellido.Text.Trim(),
                    CARGO = txtCargo.Text.Trim(),
                    INGRESO = dtIngreso.Value,
                    SALARIO = salario,
                    CORREO = txtCorreo.Text.Trim()
                };

                // Actualizar en la base de datos
                if (dbGestorEmpleados.Actualizar(empleadoActualizado))
                {
                    MessageBox.Show("Empleado actualizado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarDatos();
                    idEmpleadoSeleccionado = 0;
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el empleado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar empleado: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que se haya seleccionado un empleado
                if (idEmpleadoSeleccionado == 0)
                {
                    MessageBox.Show("Por favor, seleccione un empleado del DataGridView para eliminar",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmar eliminación
                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro que desea eliminar este empleado?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Eliminar de la base de datos
                    if (dbGestorEmpleados.Eliminar(idEmpleadoSeleccionado))
                    {
                        MessageBox.Show("Empleado eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarDatos();
                        idEmpleadoSeleccionado = 0;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el empleado", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar empleado: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCargo.Clear();
            txtSalario.Clear();
            txtCorreo.Clear();
            dtIngreso.Value = DateTime.Now;
            idEmpleadoSeleccionado = 0;
            txtNombre.Focus();
        }
    }
}