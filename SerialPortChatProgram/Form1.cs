using System.IO.Ports;

namespace SerialPortChatProgram
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort1 = new SerialPort();

        public Form1()
        {
            InitializeComponent();

            // Olay aboneliği: serialPort1 tasarımcıyla eklenmediği için
            // DataReceived'i burada elle bağlamak zorundayız.
            serialPort1.DataReceived += serialPort1_DataReceived;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var seriPort in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(seriPort);
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox2.Clear();

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.Even;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;

                serialPort1.Open();

                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                comboBox1.Enabled = false;

                textBox1.AppendText($"[{comboBox1.Text} bağlandı]{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }

                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
                comboBox1.Enabled = true;

                textBox1.AppendText($"[bağlantı kapatıldı]{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Port kapatılırken hata oluştu: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Gonder();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Gonder();
            }
        }

        private void Gonder()
        {
            if (!serialPort1.IsOpen)
            {
                return;
            }

            string mesaj = textBox2.Text;
            if (string.IsNullOrEmpty(mesaj))
            {
                return;
            }

            try
            {
                // Satır sonu, karşı tarafın mesaj sınırını bulabilmesi için.
                serialPort1.Write(mesaj + "\n");
                textBox1.AppendText($"<ben> {mesaj}{Environment.NewLine}");
                textBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gönderilemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Bu metot arayüz thread'inde DEĞİL, SerialPort'un kendi
            // arka plan thread'inde çalışır. Ekrana yazmak için Invoke şart.
            string gelen = serialPort1.ReadExisting();

            if (!IsHandleCreated || IsDisposed)
            {
                return;
            }

            textBox1.Invoke(new Action(() => textBox1.AppendText(gelen)));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.DataReceived -= serialPort1_DataReceived;

            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }

            serialPort1.Dispose();
        }
    }
}
