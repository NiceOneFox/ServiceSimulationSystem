using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ModelSystemSpecialEventMethod
{
    public partial class Waveform : Form
    {
        private int amountSource { get; set; } = 1;

        private int bufferSize { get; set; } = 1;

        private int amountDevice { get; set; } = 1;

        private int i { get; set; } = 0;


        public Waveform(int amountSource, int bufferSize, int amountDevice)
        {

            this.amountSource = amountSource;
            this.bufferSize = bufferSize;
            this.amountDevice = amountDevice;

            InitializeComponent();

        }

        private void Waveform_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.panel1.CreateGraphics();

            Brush black = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(black, 4);

            const int RANGE = 80;

            int startLineX = 40;
            int startLineY = 40;

            int interval = 40;
            int endLineX = 1800;
            int endLineY = startLineY;

            for (int i = 0; i < amountSource; i++)
            {
                g.DrawLine(blackPen, startLineX, startLineY, endLineX, endLineY);

                interval += RANGE;

                startLineY = interval;
                endLineY = interval;
            }

            Brush blue = new SolidBrush(Color.Blue);
            Pen bluePen = new Pen(blue, 4);

            for (int i = 0; i < bufferSize; i++)
            {
                g.DrawLine(bluePen, startLineX, startLineY, endLineX, endLineY);
                interval += RANGE;

                startLineY = interval;
                endLineY = interval;
            }

            Brush red = new SolidBrush(Color.Red);
            Pen redPen = new Pen(red, 4);


            g.DrawLine(redPen, startLineX, startLineY, endLineX, endLineY);
            interval += RANGE;
            startLineY = interval;
            endLineY = interval;

            Brush green = new SolidBrush(Color.Green);
            Pen greenPen = new Pen(green, 4);

            for (int i = 0; i < amountDevice; i++)
            {
                g.DrawLine(greenPen, startLineX, startLineY, endLineX, endLineY);
                interval += RANGE;

                startLineY = interval;
                endLineY = interval;
            }

           

           
            //g.DrawLine(redPen, 20.2f, 20.2f, 345.0f, 20f);

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Program.OneStep();



            if (Program.state == "Конец моделирования")
            {
                MessageBox.Show("Окончание моделирования");
                Program.CountResults();
                // TODO вывести результаты              
                return;
            }


        }
    }
}

