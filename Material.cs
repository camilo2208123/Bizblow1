using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace bizflow1._0
{
    internal class Material
    {
        public List<MaterialTemporal> materialesTemporales = new List<MaterialTemporal>();

        public class MaterialTemporal
        {
            public int IdMaterial { get; set; }
            public string NombreMaterial { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
        }

        public void CargarMaterialesComboBox(ComboBox comboBox6)
        {
            string conexion = "server=localhost;user=root;database=biz_flow;";
            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                //conn.Open();
                string query = "SELECT id_mh, nombre FROM material_herramientas";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBox6.DataSource = dt;
                comboBox6.DisplayMember = "nombre"; // Lo que se muestra
                comboBox6.ValueMember = "id_mh";       // Lo que se usa internamente



            }
        }
        public (string nombre, decimal precio) ObtenerDatosMaterial(int idMaterial)
        {
            string conexion = "server=localhost;user=root;database=biz_flow;";
            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();
                string query = "SELECT nombre, precio FROM material_herramientas WHERE id_mh = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idMaterial);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string nombre = reader.GetString("nombre");
                    decimal precio = reader.GetDecimal("precio");
                    return (nombre, precio);
                }

                return ("", 0);
            }
        }
        public void MostrarEnDataGrid(DataGridView dataGridView1)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = materialesTemporales;
        }

        public void TraerDatos_Material(string seerie, ref int id, ref double precio, ref int cantidad_u)
        {
            MySqlConnection carretera = new MySqlConnection();
            carretera.ConnectionString = "host=localhost;Uid=root;Database=biz_flow;Port=3306";
            string query = "select * from material_herramientas where nombre='" + seerie + "'";
            MySqlCommand cmd = new MySqlCommand(query, carretera);

            try
            {
                carretera.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //  string clave = reader["id"].ToString();
                    id = Convert.ToInt32(reader["id_mh"].ToString());
                    precio = Convert.ToDouble(reader["precio"].ToString());
                    cantidad_u = Convert.ToInt32(reader["unidades_disponibles"].ToString());


                }
                MessageBox.Show("material encontrado");
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                carretera.Close();
            }
        }

        public bool Insertar(string seerie, ref int id, ref double precio, ref int cantidad_u)
        {
            MySqlConnection carretera = new MySqlConnection();
            carretera.ConnectionString = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            // Usando parámetros para evitar inyección SQL
            string query = "SELECT  id_mh FROM material_herramientas WHERE nombre = @nombre";
            MySqlCommand cmd = new MySqlCommand(query, carretera);
            cmd.Parameters.AddWithValue("@nombre", seerie);

            try
            {
                carretera.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    id = reader.GetInt32("id_mh");
                    precio = reader.GetDouble("precio");
                    cantidad_u = reader.GetInt32("unidades_disponibles");
                    reader.Close();
                    return true; // Indica que se encontró el material
                }
                else
                {
                    MessageBox.Show("Material no encontrado: " + seerie);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar material: " + ex.Message);
                return false;
            }
            finally
            {
                if (carretera.State == ConnectionState.Open)
                    carretera.Close();
            }
        }


        private int? ObtenerId(MySqlConnection conn, string campoId, string tabla, string campoNombre, string valor)
        {
            string query = $"SELECT {campoId} FROM {tabla} WHERE {campoNombre} = @valor";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@valor", valor);
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    return id;
                }
                return null;
            }
        }



        public void GuardarDatosDesdeGrid(string nombreCliente, string nombreServicio, string nombreTrabajador, DataGridView dgv, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=biz_flow;Uid=root;Pwd=;"))
            {
                conn.Open();

                int? idCliente = ObtenerId(conn, "id_cliente", "cliente", "nombre", nombreCliente);
                int? idServicio = ObtenerId(conn, "id_servicio", "servicio", "nombre", nombreServicio);
                int? idTrabajador = ObtenerId(conn, "id_trabajador", "trabajador", "nombre", nombreTrabajador);



                if (idCliente == null || idServicio == null || idTrabajador == null)
                {
                    MessageBox.Show("No se pudo obtener uno de los IDs necesarios.");
                    return;
                }

                foreach (DataGridViewRow fila in dgv.Rows)
                {
                    if (fila.IsNewRow) continue;

                    object valorCelda = fila.Cells["id"].Value;

                    if (valorCelda == null || valorCelda == DBNull.Value)
                    {
                        MessageBox.Show("El campo 'id' está vacío en alguna fila. Se omitirá esa fila.");
                        continue; // Salta a la siguiente fila
                    }

                    if (!int.TryParse(valorCelda.ToString(), out int idMaterial))
                    {
                        MessageBox.Show($"El valor '{valorCelda}' en la columna 'id' no es un número válido. Se omitirá esa fila.");
                        continue; // Salta a la siguiente fila
                    }

                    // Lo mismo para unidades
                    object valorUnidades = fila.Cells["total_unidades"].Value;
                    if (valorUnidades == null || valorUnidades == DBNull.Value)
                    {
                        MessageBox.Show("El campo 'total_unidades' está vacío. Se omitirá esa fila.");
                        continue;
                    }

                    if (!int.TryParse(valorUnidades.ToString(), out int unidades))
                    {
                        MessageBox.Show($"El valor '{valorUnidades}' en 'total_unidades' no es válido. Se omitirá esa fila.");
                        continue;
                    }

                    // Aquí ya tienes idMaterial y unidades seguros para usar
                    // Tu código para insertar:
                    MySqlCommand cmd = new MySqlCommand(
                        @"INSERT INTO of_ti (fk_cliente, fk_servicio, fk_trabajador, fk_material, unidades_ocupadas, fecha_i, fecha_f, estado_servicio)
          VALUES (@cliente, @servicio, @trabajador, @material, @unidades, @fechaI, @fechaF, @estado)", conn);

                    cmd.Parameters.AddWithValue("@cliente", idCliente);
                    cmd.Parameters.AddWithValue("@servicio", idServicio);
                    cmd.Parameters.AddWithValue("@trabajador", idTrabajador);
                    cmd.Parameters.AddWithValue("@material", idMaterial);
                    cmd.Parameters.AddWithValue("@unidades", unidades);
                    cmd.Parameters.AddWithValue("@fechaI", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaF", fechaFin);
                    cmd.Parameters.AddWithValue("@estado", estado);

                    cmd.ExecuteNonQuery();
                }


                MessageBox.Show("Registros insertados correctamente.");
            }
        }

        public string Anadir_Material(string nombre, double precio, DateTime fecha, double unidades)
        {
            string mensaje = "";
            string cadenaConexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            try
            {

                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {

                    string query = @"
            INSERT INTO material_herramientas (nombre, precio, unidades_disponibles, fecha_registro) VALUES (@nombre, @precio, @unidades, @fecha)";
                    //  string query = "DELETE FROM usuario WHERE id = @id";
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
                    cmd.Parameters.Add("@precio", MySqlDbType.Double).Value = precio;
                    cmd.Parameters.Add("@unidades", MySqlDbType.Double).Value = unidades;
                    cmd.Parameters.Add("@fecha", MySqlDbType.DateTime).Value = fecha;
                    cmd.ExecuteNonQuery();
                    connection.Close();

                }
                mensaje = "k";
                MessageBox.Show("✅ Guardado exitosamente");
            }
            catch (Exception men)
            {
                mensaje = "Error:" + men.Message;
            }


            return mensaje;


        }
        public void consulta_material(DataGridView dh)
        {
            MySqlConnection carretera = new MySqlConnection();
            carretera.ConnectionString = "server=localhost; uid=root;database=biz_flow;Port=3306;";

            string query = "select * from material_herramientas";
            MySqlCommand cmd = new MySqlCommand(query, carretera);
            carretera.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            DataTable tabla = new DataTable();
            tabla.Load(dr);
            dh.DataSource = tabla;
            carretera.Close();

        }
        public void TraerDatos_Material(int seerie, ref string nom, ref double precio, ref double unidad, ref DateTime fecha)
        {
            MySqlConnection carretera = new MySqlConnection();
            carretera.ConnectionString = "host=localhost;Uid=root;Database=biz_flow;Port=3306";
            string query = "select * from material_herramientas where id_mh=" + seerie;
            MySqlCommand cmd = new MySqlCommand(query, carretera);

            try
            {
                carretera.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //  string clave = reader["id"].ToString();
                    nom = reader["nombre"].ToString();
                    precio = Convert.ToDouble(reader["precio"]);
                    unidad = Convert.ToDouble(reader["unidades_disponibles"]);
                    fecha = Convert.ToDateTime(reader["fecha_registro"]);

                }
                MessageBox.Show("material encontrado");
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                carretera.Close();
            }
        }
        public string Actualizar_Material(int serie, int cantidad, double precio, DateTime fecha)
        {
            string mensaje = "";
            string cadenaConexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            try
            {

                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    string query = "UPDATE material_herramientas SET unidades_disponibles=@cantidad, precio=@precio, fecha_registro=@fecha WHERE id_mh =" + serie;
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@fecha", fecha);

                    //  cmd.Parameters.AddWithValue("@id", clave);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Modificación exitosa");

            }
            catch (Exception men)
            {
                mensaje = "Error:" + men.Message;
            }


            return mensaje;


        }
        public string EliminarMH(int idmh)
        {
            string mensaje = "";
            string cadenaConexion = "server=localhost;Uid=root;Database=biz_flow;Port=3306";

            using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM material_herramientas WHERE id_mh = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = idmh;
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        mensaje = filasAfectadas > 0
                            ? "que pro registro eliminado correctamente"
                            : "bot no se encontró un registro con ese ID escriba bien.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = " Error al eliminar: " + ex.Message;
                }
            }

            return mensaje;

        }//eliminar datos 

        public void CambiarEstadoYFecha(int idCliente, string nuevoEstado, DateTime nuevaFechaFin)
        {
            string cadenaConexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";
            string query = @" UPDATE of_ti SET estado_servicio = @estado,fecha_f = @fechaFin WHERE fk_cliente = @idCliente";

            using (MySqlConnection conn = new MySqlConnection(cadenaConexion))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@fechaFin", nuevaFechaFin);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Cambio de estado exitoso");
        }
        public void Buscarporesatdo(string estado, DataGridView dgv)
        {
                    
            string conexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";         

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                string query = @"SELECT 
                            o.fk_cliente AS 'ID Cliente', 
                            s.nombre AS 'Trabajo Realizado', 
                            o.estado_servicio AS 'Estado'
                         FROM 
                            of_ti o
                         JOIN 
                            servicio s ON o.fk_servicio = s.id_servicio
                         WHERE 
                            o.estado_servicio = @estado";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", estado);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
            }
        }
        public void BuscarTrabajadores(bool disponibles, DataGridView dgv)
        {
        
            string conexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";
            string query;

            if (disponibles)
            {
                // Trabajadores DISPONIBLES
                query = @"SELECT t.id_trabajador, t.nombre
                  FROM trabajador t
                  WHERE t.id_trabajador NOT IN (
                      SELECT fk_trabajador
                      FROM of_ti
                      WHERE estado_servicio = 'Pendiente' OR estado_servicio = 'En Proceso'
                  )";
            }
            else
            {
                // Trabajadores OCUPADOS
                query = @"SELECT DISTINCT t.id_trabajador, t.nombre
                  FROM trabajador t
                  JOIN of_ti o ON t.id_trabajador = o.fk_trabajador
                  WHERE o.estado_servicio = 'Pendiente' OR o.estado_servicio = 'En Proceso'";
            }

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
            }
        }
        public DataTable ObtenerClientes()
        {
            string connectionString = "host=localhost;Uid=root;Database=biz_flow;Port=3306";
            DataTable tabla = new DataTable();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                string consulta = "SELECT id_cliente, nombre, numero_telefonico, correo_electronico FROM cliente";

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    conexion.Open();
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

    }

}



