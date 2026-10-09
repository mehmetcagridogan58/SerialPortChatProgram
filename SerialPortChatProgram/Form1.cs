using System.Diagnostics;
using System.IO.Ports;

namespace SerialPortChatProgram
{
    public partial class Form1 : Form
    {

        private SerialPort serialPort1 = new SerialPort();

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var seriPort in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(seriPort);
            }

            button1.Enabled = true;
            button2.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                serialPort1.PortName = comboBox1.Text;
                // Baud Rate: 9600
                serialPort1.BaudRate = 9600;
                // This port uses 8E1
                serialPort1.Parity = Parity.Even;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;

                serialPort1.Open();

                MessageBox.Show("Port Bağlantısı Gerçekleşti.");

                button1.Enabled = false;
                button2.Enabled = true;
                comboBox1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı Hatası" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try 
            {
                serialPort1.Close();

                MessageBox.Show("Bağlantı kapatıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                button1.Enabled = true;
                button2.Enabled = false;
                comboBox1.Enabled = true;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Port kapatılırken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1 != null && serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

    }
}
