using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace bizflow1._0
{
    internal class Servicios
    {
      

      public void CargarServiciosPorOficio(string oficio, ComboBox comboBoxServicios)
     {
        string connectionString = "server=localhost;user=root;database=prueba_final;port=3306;password=tu_password";

        string query = "SELECT nombre_servicio FROM tipo_servicio WHERE tipo_de_servicio = @oficio";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@oficio", oficio);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBoxServicios.Items.Clear();

                        while (reader.Read())
                        {
                            comboBoxServicios.Items.Add(reader.GetString("nombre_servicio"));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar servicios: " + ex.Message);
        }
      }

        public string Guardar_cliente(  string correo, string numero, string nomb, string calle, string colonia, string codigopostal, string municipio, string tipoVivienda,
                                           string numeroInterior, string numeroExterior)
        {
            string mensaje = "";
            string cadenaConexion = "server=localhost;Uid=root;Database=biz_flow;Port=3306";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();

                    // 1. Insertar en direccion_cliente
                    string queryDireccion = @"INSERT INTO direccion 
                (calle, colonia, numero_int, numero_ext, codigo_postal, municipio, tipo_vivienda) 
                VALUES (@calle, @colonia, @numi, @nume, @cp, @mun, @tipv)";

                    MySqlCommand cmdDireccion = new MySqlCommand(queryDireccion, connection);
                    cmdDireccion.Parameters.AddWithValue("@calle", calle);
                    cmdDireccion.Parameters.AddWithValue("@colonia", colonia);
                    cmdDireccion.Parameters.AddWithValue("@cp", codigopostal);
                    cmdDireccion.Parameters.AddWithValue("@mun", municipio);
                    cmdDireccion.Parameters.AddWithValue("@tipv", tipoVivienda);
                    cmdDireccion.Parameters.AddWithValue("@numi", numeroInterior);
                    cmdDireccion.Parameters.AddWithValue("@nume", numeroExterior);
                    cmdDireccion.ExecuteNonQuery();

                    long id_direccion = cmdDireccion.LastInsertedId; // ✅ Aquí obtienes el ID insertado

                    // 2. Insertar en cliente
                    string queryCliente = @"INSERT INTO cliente ( nombre, numero_telefonico, correo_electronico, id_direccion  ) 
                                    VALUES (@nombre, @num, @correo, @idDireccion)";

                    MySqlCommand cmdCliente = new MySqlCommand(queryCliente, connection);
                    cmdCliente.Parameters.AddWithValue("@nombre", nomb);
                    cmdCliente.Parameters.AddWithValue("@num", numero);
                    cmdCliente.Parameters.AddWithValue("@correo", correo);
                    cmdCliente.Parameters.AddWithValue("@idDireccion", id_direccion);
                    cmdCliente.ExecuteNonQuery();

                    connection.Close();

                    mensaje = "Modificación exitosa";
                    MessageBox.Show("✅ Cliente y dirección registrados correctamente");
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error: " + ex.Message;
                MessageBox.Show(mensaje);
            }

            return mensaje;
        }

        public void Ser(ComboBox tipo, ComboBox serv)
        {

            serv.Items.Clear(); // Limpia los servicios anteriores
            string tipoOficio = tipo.Text.Trim(); // Elimina espacios extra

            switch (tipoOficio)
            {
                case "Electricista":
                    serv.Items.Add("Instalación de contactos y apagadores");
                    serv.Items.Add("Instalación de lámparas y focos");
                    serv.Items.Add("Revisión y reparación de cortos eléctricos");
                    break;

                case "Plomero":
                    serv.Items.Add("Destapar cañerías");
                    serv.Items.Add("Reparación de fuga en tuberías");
                    serv.Items.Add("Cambio de válvulas o llaves de paso");
                    break;

                case "Carpintero":
                    serv.Items.Add("Instalación de puertas de madera");
                    serv.Items.Add("Colocación de closets o alacenas");
                    serv.Items.Add("Restauración de madera");
                    break;

                case "Mecanico":
                    serv.Items.Add("Diagnóstico de fallas mecánicas");
                    serv.Items.Add("Reparación de motor");
                    serv.Items.Add("Cambio de aceite y filtro");
                    break;

                case "Tecnico":
                    serv.Items.Add("Instalación de cámaras o routers");
                    serv.Items.Add("Reparación de lavadoras o microondas");
                    serv.Items.Add("Mantenimiento de aire acondicionado");
                    break;

                default:
                    serv.Items.Add("Sin servicios disponibles");
                    break;
            }

            serv.SelectedIndex = -1; // Limpia la selección actual
        }
        public int Pre(ComboBox tip, ComboBox ser, ref int precio)
        {
            string tipo = tip.Text.Trim();
            string servicio = ser.Text.Trim();

            switch (tipo)
            {
                case "Electricista":
                    switch (servicio)
                    {
                        case "Instalación de contactos y apagadores":
                            precio = 150;


                            break;
                        case "Instalación de lámparas y focos":
                            precio = 200;

                            break;
                        case "Revisión y reparación de cortos eléctricos":
                            precio = 250;

                            break;
                    }
                    break;

                case "Plomero":
                    switch (servicio)
                    {
                        case "Destapar cañerías":
                            precio = 180;

                            break;
                        case "Reparación de fuga en tuberías":
                            precio = 220;

                            break;
                        case "Cambio de válvulas o llaves de paso":
                            precio = 160;

                            break;
                    }
                    break;
                case "Carpintero":
                    switch (servicio)
                    {
                        case "Instalación de puertas de madera":
                            precio = 700;

                            break;
                        case "Colocación de closets o alacenas":
                            precio = 1500;

                            break;
                        case "Restauración de madera":
                            precio = 500;

                            break;
                    }
                    break;
                case "Mecanico":
                    switch (servicio)
                    {
                        case "Diagnóstico de fallas mecánicas":
                            precio = 300;

                            break;
                        case "Reparación de motor":
                            precio = 2000;

                            break;
                        case "Cambio de aceite y filtro":
                            precio = 400;


                            break;
                    }
                    break;
                case "Tecnico":
                    switch (servicio)
                    {
                        case "Instalación de cámaras o routers":
                            precio = 800;

                            break;
                        case "Reparación de lavadoras o microondas":
                            precio = 500;

                            break;
                        case "Mantenimiento de aire acondicionado":
                            precio = 600;

                            break;
                    }
                    break;


            }


            return (precio);
        }
        public void CargarTrabajadoresPorTipoServicio(string tipoServicio, ComboBox combo)
        {
            string conexion = "server=localhost;user=root;database=biz_flow;port=3306";

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                string query = "SELECT nombre FROM trabajador WHERE oficio = @tipo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tipo", tipoServicio);

                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    combo.DataSource = dt;
                    combo.DisplayMember = "nombre"; // Lo que se muestra en el ComboBox
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar trabajadores: " + ex.Message);
                }
            }
        }

        public bool Guardar_cliente(string nombre, ref int id)
        {

            
            MySqlConnection carretera = new MySqlConnection();
            carretera.ConnectionString = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            // Usando parámetros para evitar inyección SQL
            string query = "SELECT  id FROM cliente WHERE nombre = @nombre";
            MySqlCommand cmd = new MySqlCommand(query, carretera);
            cmd.Parameters.AddWithValue("@nombre", nombre);

            try
            {
                carretera.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
             
                   
                    reader.Close();
                    return true; // Indica que se encontró el material
                }
                else
                {
                    MessageBox.Show("Material no encontrado: " + nombre);
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

        public string Guardar_trabajador(string nomb, string correo,string numero, string direccion, string oficcio)
        {
            string mensaje = "";
            string cadenaConexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();

                    // 1. Insertar en tabla trabajador
                    string query = @"INSERT INTO trabajador 
                             (nombre, oficio, direccion, celular,correo) 
                             VALUES (@nomb, @oficcio, @direccion, @celular, @correo)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);                   
                    cmd.Parameters.Add("@nomb", MySqlDbType.VarChar).Value = nomb;
                    cmd.Parameters.Add("@oficcio", MySqlDbType.VarChar).Value = oficcio;
                    cmd.Parameters.Add("@direccion", MySqlDbType.VarChar).Value = direccion;
                    cmd.Parameters.Add("@celular", MySqlDbType.VarChar).Value = numero;
                    cmd.Parameters.Add("@correo", MySqlDbType.VarChar).Value = correo;                   
                    cmd.ExecuteNonQuery(); // ✅ ejecutar el primer insert             
                }

                mensaje = "Modificación exitosa";
                MessageBox.Show("✅ Usuario creado");
            }
            catch (Exception men)
            {
                mensaje = "Error: " + men.Message;
                MessageBox.Show(mensaje);
            }

            return mensaje;
        }
        public (string clienteNombre, string servicios, string trabajadores, string materiales, string estado, DateTime fechaInicio) BuscarDatosCliente(int idCliente)
        {
            string cadenaConexion = "host=localhost;Uid=root;Database=biz_flow;Port=3306";

            string clienteNombre = "";
            string servicios = "";
            string trabajadores = "";
            string materiales = "";
            string estado = "";
            DateTime fechaInicio = DateTime.MinValue;

            string query = @"
        SELECT 
            c.nombre AS cliente,
            s.nombre AS servicio,
            t.nombre AS trabajador,
            SUM(mh.precio * o.unidades_ocupadas) AS total_materiales,
            o.estado_servicio,
            o.fecha_i
        FROM of_ti o
        JOIN cliente c ON o.fk_cliente = c.id_cliente
        JOIN servicio s ON o.fk_servicio = s.id_servicio
        JOIN trabajador t ON o.fk_trabajador = t.id_trabajador
        JOIN material_herramientas mh ON o.fk_material = mh.id_mh
        WHERE o.fk_cliente = @idCliente
        GROUP BY c.nombre, s.nombre, t.nombre, o.estado_servicio, o.fecha_i";

            using (MySqlConnection conn = new MySqlConnection(cadenaConexion))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clienteNombre = reader["cliente"].ToString();
                        servicios = reader["servicio"].ToString();
                        trabajadores = reader["trabajador"].ToString();
                        materiales = reader["total_materiales"].ToString();
                        estado = reader["estado_servicio"].ToString();
                        fechaInicio = Convert.ToDateTime(reader["fecha_i"]);
                    }
                }
            }

            return (clienteNombre, servicios, trabajadores, materiales, estado, fechaInicio);
        }


    }
}
