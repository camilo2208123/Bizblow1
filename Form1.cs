using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static bizflow1._0.Material;
using System.Media;
using ZstdSharp.Unsafe;

namespace bizflow1._0
{
    public partial class Form1 : Form
    {
        //List<MaterialTemporal> materialesTemporales = new List<MaterialTemporal>();


        public Form1()
        {
            InitializeComponent();
            comboBox6.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
        }

        Servicios cliente = new Servicios();
        Material herramienta = new Material();


        private void Form1_Load(object sender, EventArgs e)
        {
            herramienta.CargarMaterialesComboBox(comboBox6);
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            string nombre = textBox1.Text.Trim();
            string correo = textBox2.Text.Trim();
            string telefono = textBox7.Text.Trim();
            // Dirección
            string calle = textBox3.Text.Trim();
            string colonia = textBox4.Text.Trim();
            string cp = textBox8.Text.Trim();
            string municipio = textBox17.Text.Trim();
            string tipoVivienda = comboBox1.Text;
            string numeroInterior = textBox5.Text.Trim();
            string numeroExterior = textBox6.Text.Trim();
            textBox16.Text = textBox1.Text;
            // Llamamos al método
            cliente.Guardar_cliente(correo, telefono, nombre, calle, colonia, cp, municipio, tipoVivienda, numeroInterior, numeroExterior);
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 3;

            // Opcionalmente puedes mostrar el mensaje aquí también

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            cliente.Ser(comboBox2, comboBox3);

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int precio = 0;
            cliente.Pre(comboBox2, comboBox3, ref precio);
            textBox9.Text = "$" + precio.ToString();

        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {



        }


        private void button11_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu12.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            // Crear nueva fila
            DataGridViewRow fila = new DataGridViewRow();
            fila.CreateCells(dataGridView1);

            // Obtener datos
            string producto = textBox13.Text;
            string categoria = comboBox6.Text;
            int precio = Convert.ToInt32(textBox12.Text);
            int cantidad = Convert.ToInt32(textBox18.Text);
            int totalPorFila = precio * cantidad;

            // Llenar celdas
            fila.Cells[0].Value = categoria;
            fila.Cells[1].Value = producto;
            fila.Cells[2].Value = precio.ToString();
            fila.Cells[3].Value = cantidad.ToString();
            fila.Cells[4].Value = totalPorFila.ToString();

            // Agregar fila al DataGridView
            dataGridView1.Rows.Add(fila);

            // Limpiar campos
            textBox13.Clear();
            comboBox6.Text = "";
            textBox12.Clear();
            textBox18.Clear();

            // Recalcular total general
            double totalGeneral = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                totalGeneral += Convert.ToDouble(row.Cells[4].Value);
            }

            // Mostrar total general en textBox10
            textBox10.Text = totalGeneral.ToString("F2");

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu12.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            int cantidad = 0;
            string serie = comboBox6.Text.Trim();
            int id = 0;           
            double precio = 0;

            herramienta.TraerDatos_Material(serie, ref id, ref precio, ref cantidad);
            //MessageBox.Show($"ID: {id}, Precio: {precio}, Cantidad: {cantidad}");
            textBox11.Text = cantidad.ToString();
            textBox13.Text = id.ToString();          
            textBox12.Text = precio.ToString();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox14.Text = comboBox2.Text;
            textBox15.Text = comboBox3.Text;
            string oficio = comboBox2.Text;
            cliente.CargarTrabajadoresPorTipoServicio(oficio, comboBox4);
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 4;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button16_Click(object sender, EventArgs e)
        {

            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila primero.");
                return;
            }

            // Obtener valor de la columna (por índice o nombre)
            object valorColumnaObj = dataGridView1.CurrentRow.Cells["Column1"].Value; // o usa índice: Cells[3]

            if (valorColumnaObj == null || string.IsNullOrEmpty(valorColumnaObj.ToString()))
            {
                MessageBox.Show("La celda seleccionada está vacía.");
                return;
            }

            // Intentar convertir los valores a double
            if (!double.TryParse(valorColumnaObj.ToString(), out double valorColumna))
            {
                MessageBox.Show("Valor en la columna no es un número válido.");
                return;
            }

            if (!double.TryParse(textBox10.Text, out double valorTextBox))
            {
                MessageBox.Show("Valor en el TextBox no es un número válido.");
                return;
            }

            // Hacer la resta
            double resultado = valorTextBox - valorColumna;

            // Mostrar resultado en otro TextBox
            textBox10.Text = resultado.ToString("F2");
       

             dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void button13_Click(object sender, EventArgs e)
        {

            string servicio = textBox15.Text;
            string cliente = textBox16.Text;
            string trabajador = comboBox4.Text;
            string estado_ser = comboBox5.Text;
           
            herramienta.GuardarDatosDesdeGrid(  cliente, servicio, trabajador, dataGridView1, dateTimePicker1.Value, dateTimePicker2.Value, estado_ser);
          
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu12.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu22.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 3;
        }

        private void button12_Click(object sender, EventArgs e)
        {
        
            textBox19.Text = textBox10.Text;
            textBox20.Text = textBox9.Text;      
            double precioMaterial = Convert.ToDouble(textBox10.Text.Trim());
            double precioServicio = Convert.ToDouble(textBox9.Text.Replace("$", "").Trim());

            // Sumar
            double suma = precioMaterial + precioServicio;

            // Mostrar resultados
            textBox19.Text = textBox10.Text;
            textBox20.Text = textBox9.Text;
            textBox21.Text = "$ " + suma.ToString(); // Muestra con 2 decimales

            // Cambiar de pestaña
            tabControl1.SelectedIndex = 5;

            textBox21.Text = suma.ToString();
 
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex =5 ;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox27.Text = comboBox2.Text;
            textBox26.Text = comboBox3.Text;
            string oficio = comboBox2.Text;
            tabControl1.SelectedIndex = 4;
        }

        private void button21_Click(object sender, EventArgs e)
        {

            textBox23.Text = textBox10.Text;
            textBox24.Text = textBox9.Text;
            double precioMaterial = Convert.ToDouble(textBox10.Text.Trim());
            double precioServicio = Convert.ToDouble(textBox9.Text.Replace("$", "").Trim());

            // Sumar
            double suma = precioMaterial + precioServicio;

            // Mostrar resultados
            textBox24.Text = textBox10.Text;
            textBox23.Text = textBox9.Text;
            textBox22.Text = "$ " + suma.ToString(); // Muestra con 2 decimales      

            textBox21.Text = suma.ToString();

            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
        
            tabControl1.SelectedIndex = 6;

        }

        private void button19_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {


           
        }

        private void button27_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();           
            string nombre=textBox43.Text;
            string correo = textBox42.Text;
            string direccion = textBox41.Text;
            string num = textBox37.Text;
            string of= comboBox9.Text;
            cliente.Guardar_trabajador(nombre,correo, num, direccion,of );
        }

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            string nombre = textBox28.Text;
            double precio = Convert.ToDouble(textBox29.Text);
            double unidades = Convert.ToDouble(textBox30.Text);
            DateTime fecha = dateTimePicker5.Value;
            herramienta.Anadir_Material(nombre, precio,fecha , unidades);

        }

        private void button30_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            herramienta.consulta_material(dataGridView2);
            tabControl1.SelectedIndex = 9;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
           
            tabControl1.SelectedIndex = 7;
        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void button31_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();

            int cantidad = Convert.ToInt32(textBox31.Text);      // Unidades
            double precio = Convert.ToDouble(textBox32.Text);     // Precio
            int id = Convert.ToInt32(textBox34.Text);             // ID del material
            DateTime fecha = dateTimePicker6.Value;               // Fecha de registro

            herramienta.Actualizar_Material(id, cantidad, precio, fecha);
        }

        

        private void button25_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            int id = Convert.ToInt32(textBox34.Text);
            string nombre = "";
            double precio = 0;
            double unidades = 0;
            DateTime fecha = DateTime.MinValue;
            herramienta.TraerDatos_Material( id,ref nombre, ref precio, ref unidades, ref  fecha);
            textBox33.Text=nombre;
            textBox32.Text=precio.ToString();
            textBox31.Text=unidades.ToString();
            dateTimePicker6.Value = fecha;


        }

        private void button32_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            int id = Convert.ToInt32(textBox34.Text);
            herramienta.EliminarMH(id);

        }

        private void button34_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
           

            try
            {
                int idCliente = int.Parse(textBox39.Text); // Por ejemplo, un TextBox donde escribes el ID

                Servicios servicioDAO = new Servicios();

                var resultado = servicioDAO.BuscarDatosCliente(idCliente);

                // Mostrar resultados en controles de tu formulario
                textBox40.Text = resultado.servicios;
                comboBox11.Text = resultado.trabajadores;
                //textBox38.Text = resultado.materiales;
                comboBox10.Text = resultado.estado;
                dateTimePicker8.Value=resultado.fechaInicio;
                textBox45.Text =resultado.clienteNombre;
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingresa un ID válido (número).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            int cliente = Convert.ToInt32(textBox39.Text);
            string estado = comboBox10.Text;
            DateTime fecha = dateTimePicker7.Value;
            herramienta.CambiarEstadoYFecha(cliente, estado, fecha);
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 12;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 7;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 10;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button37_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 8;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            textBox39.Clear();          // ID del cliente
            textBox40.Clear();          // Servicios
            comboBox11.SelectedIndex = -1; // Trabajadores (dejar sin selección)
            comboBox10.SelectedIndex = -1; // Estado (dejar sin selección)
            dateTimePicker8.Value = DateTime.Now; // Fecha de inicio a hoy
            textBox45.Clear();
            
        }

        private void button36_Click(object sender, EventArgs e)
        {
            textBox39.Clear();          // ID del cliente
            textBox40.Clear();          // Servicios
            comboBox11.SelectedIndex = -1; // Trabajadores (dejar sin selección)
            comboBox10.SelectedIndex = -1; // Estado (dejar sin selección)
            dateTimePicker8.Value = DateTime.Now; // Fecha de inicio a hoy
            textBox45.Clear();
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button41_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 11;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            herramienta.Buscarporesatdo("Pendiente", dataGridView5);
            tabControl1.SelectedIndex = 17;
        }

        private void button40_Click(object sender, EventArgs e)
        {
            herramienta.Buscarporesatdo("Realizado", dataGridView3);
            tabControl1.SelectedIndex = 15;
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            herramienta.Buscarporesatdo("En proceso", dataGridView4);
            tabControl1.SelectedIndex = 16;
        }

        private void button42_Click(object sender, EventArgs e)
        {
           
        }

        private void button43_Click(object sender, EventArgs e)
        {
            herramienta.BuscarTrabajadores(false, dataGridView7);
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 18;
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {

        }

        private void button44_Click(object sender, EventArgs e)
        {
            herramienta.BuscarTrabajadores(true, dataGridView6);
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 14;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button52_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button53_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void Regresar_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 12;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 14;
        }

        private void button56_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 12;
        }

        private void button57_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button48_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 12;
        }

        private void button49_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 16;
        }

        private void button51_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 17;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button55_Click(object sender, EventArgs e)
        {
           
            dataGridView8.DataSource = herramienta.ObtenerClientes();
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 13;
        }

        private void button42_Click_1(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu3.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            string sonidoRuta = @"C:\Users\moral\Downloads\sfx-menu5.wav";

            SoundPlayer sonido = new SoundPlayer(sonidoRuta);
            sonido.Play();
            tabControl1.SelectedIndex = 1;
        }
    }
    
    
}

        
    








