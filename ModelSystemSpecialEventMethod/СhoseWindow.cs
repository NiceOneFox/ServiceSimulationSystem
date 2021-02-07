using System;
using System.Windows.Forms;

namespace ModelSystemSpecialEventMethod
{
    public partial class Choose : Form
    {
        public Choose()
        {
            InitializeComponent();

        }

        private void СhoseWindow_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numOfSources = int.Parse(textBox1.Text.Trim());
                int bufSize = int.Parse(textBox2.Text.Trim());
                int numOfDevice = int.Parse(textBox3.Text.Trim());
                int N = int.Parse(textBox4.Text.Trim());

                Program.ClearOutputParameters();
                Program.InitilizeInputParameters(numOfSources, bufSize, numOfDevice, N);

                Program.InitiliazeRequestsRange(float.Parse(textBox5.Text.Trim()));

                Form1 form1 = new Form1();
                form1.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int numOfSources = int.Parse(textBox1.Text.Trim());
                int bufSize = int.Parse(textBox2.Text.Trim());
                int numOfDevice = int.Parse(textBox3.Text.Trim());
                int N = int.Parse(textBox4.Text.Trim());
                Program.ClearOutputParameters();
                Program.InitilizeInputParameters(numOfSources, bufSize, numOfDevice, N);

                Program.InitiliazeRequestsRange(float.Parse(textBox5.Text.Trim()));

                //float Pfailure = float.Parse(textBox6.Text.Trim());
               // float workloadDevice = float.Parse(textBox7.Text.Trim());
                    
                Auto autoMode = new Auto();
                autoMode.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                int numOfSources = int.Parse(textBox1.Text.Trim());
                int bufSize = int.Parse(textBox2.Text.Trim());
                int numOfDevice = int.Parse(textBox3.Text.Trim());
                int N = int.Parse(textBox4.Text.Trim());
                Program.InitilizeInputParameters(numOfSources, bufSize, numOfDevice, N);

                Waveform waveform = new Waveform(numOfSources, bufSize, numOfDevice);
                waveform.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
    }
}
