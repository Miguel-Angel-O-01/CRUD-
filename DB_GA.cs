using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_GA
{
    internal class DB_GA
    {
        static string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS; database=GRUPO_ARCENSA; Integrated Security=True";

        // Método para obtener todos los empleados
        public List<EMPLEADO> Get()
        {
            List<EMPLEADO> empleados = new List<EMPLEADO>();
            string query = "SELECT * FROM EMPLEADOS";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EMPLEADO empleado = new EMPLEADO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NOMBRE = reader["NOMBRE"].ToString(),
                            APELLIDO = reader["APELLIDO"].ToString(),
                            CARGO = reader["CARGO"].ToString(),
                            SALARIO = Convert.ToDecimal(reader["SALARIO"]),
                            CORREO = reader["CORREO"].ToString(),
                            INGRESO = Convert.ToDateTime(reader["INGRESO"])
                        };
                        empleados.Add(empleado);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al conectar con la base de datos: " + ex.Message);
                }
            }
            return empleados;
        }

        // Método para agregar un empleado
        public bool Agregar(EMPLEADO empleado)
        {
            string query = "INSERT INTO EMPLEADOS (NOMBRE, APELLIDO, CARGO, INGRESO, SALARIO, CORREO) " +
                          "VALUES (@NOMBRE, @APELLIDO, @CARGO, @INGRESO, @SALARIO, @CORREO)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NOMBRE", empleado.NOMBRE);
                command.Parameters.AddWithValue("@APELLIDO", empleado.APELLIDO);
                command.Parameters.AddWithValue("@CARGO", empleado.CARGO);
                command.Parameters.AddWithValue("@INGRESO", empleado.INGRESO);
                command.Parameters.AddWithValue("@SALARIO", empleado.SALARIO);
                command.Parameters.AddWithValue("@CORREO", empleado.CORREO);

                try
                {
                    connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar empleado: " + ex.Message);
                }
            }
        }

        // Método para actualizar un empleado
        public bool Actualizar(EMPLEADO empleado)
        {
            string query = "UPDATE EMPLEADOS SET NOMBRE=@NOMBRE, APELLIDO=@APELLIDO, " +
                          "CARGO=@CARGO, INGRESO=@INGRESO, SALARIO=@SALARIO, CORREO=@CORREO " +
                          "WHERE ID=@ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", empleado.ID);
                command.Parameters.AddWithValue("@NOMBRE", empleado.NOMBRE);
                command.Parameters.AddWithValue("@APELLIDO", empleado.APELLIDO);
                command.Parameters.AddWithValue("@CARGO", empleado.CARGO);
                command.Parameters.AddWithValue("@INGRESO", empleado.INGRESO);
                command.Parameters.AddWithValue("@SALARIO", empleado.SALARIO);
                command.Parameters.AddWithValue("@CORREO", empleado.CORREO);

                try
                {
                    connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar empleado: " + ex.Message);
                }
            }
        }

        // Método para eliminar un empleado
        public bool Eliminar(int id)
        {
            string query = "DELETE FROM EMPLEADOS WHERE ID=@ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar empleado: " + ex.Message);
                }
            }
        }
    }

    public class EMPLEADO
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }
        public string CARGO { get; set; }
        public DateTime INGRESO { get; set; }
        public decimal SALARIO { get; set; }
        public string CORREO { get; set; }
    }
}