using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelSystemSpecialEventMethod
{
    public partial class Form1 : Form
    {

        private int index { get; set; } = 0;
        public Form1()
        {
            InitializeComponent();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.InitializeInputData();

            // buffer
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = Program.bufferSize;
            dataGridView1.Columns[0].HeaderText = "№ в буфере";
            dataGridView1.Columns[1].HeaderText = "Состояние";
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
            for (int j = 0; j < Program.bufferSize; j++)
            {
                dataGridView1.Rows[j].Cells[0].Value = "Свободен";
            }
            
            //devices
            dataGridView3.ColumnCount = 2;
            dataGridView3.RowCount = Program.devices.Count();
            dataGridView3.Columns[0].HeaderText = "№ прибора";
            dataGridView3.Columns[1].HeaderText = "Состояние";
            dataGridView3.Columns[0].Width = 100;
            dataGridView3.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < Program.devices.Count(); i++)
            {
                dataGridView3.Rows[i].Cells[0].Value = i;
                dataGridView3.Rows[i].Cells[1].Value = "Свободен";

            }

            // sources
            dataGridView2.ColumnCount = 4;
            dataGridView2.RowCount = Program.amountSouce;
            dataGridView2.Columns[0].HeaderText = "№ источника";
            dataGridView2.Columns[1].HeaderText = "Заявка";
            dataGridView2.Columns[0].Width = 100;
            dataGridView2.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns[2].HeaderText = "Номер заявки";

            dataGridView2.Columns[3].HeaderText = "Приоритет";
            dataGridView2.Columns[3].Width = 80;

            for (int i = 0; i < Program.amountSouce; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = i;
                dataGridView2.Rows[i].Cells[1].Value = "Свободен";
                dataGridView2.Rows[i].Cells[2].Value = "-";
                dataGridView2.Rows[i].Cells[3].Value = Program.sources.ElementAt(i).priority;
            }

            //current state
            dataGridView4.ColumnCount = 2;
            dataGridView4.Columns[0].HeaderText = "Событие";
            dataGridView4.Columns[1].HeaderText = "Информация";
            dataGridView4.Columns[0].Width = 250;
            dataGridView4.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;

            //results
            dataGridView5.ColumnCount = 2;
            dataGridView5.Columns[0].HeaderText = "Параметр";
            dataGridView5.Columns[1].HeaderText = "Значение";
            dataGridView5.Columns[0].Width = 260;
            dataGridView5.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
            //parameters of system               
            dataGridView5.Rows.Add("Количество источников: ",Program.amountSouce);
            dataGridView5.Rows.Add("Размер буфера: ", Program.bufferSize);
            dataGridView5.Rows.Add("Количество приборов: ", Program.amountDevice);

            //time
            textBox1.Text = Program.time.ToString("F8");
         

        }
      
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ShowResults()
        {
            for (int i = 0; i < Program.finalResults.Count(); i++)
            {

                StringBuilder parameter = new StringBuilder(16);
                StringBuilder resultOfParameter = new StringBuilder(26);
                bool wasSeparator = false;
                string currentResult = Program.finalResults[i];
                for (int j = 0; j < currentResult.Length; j++)
                {
                    if (currentResult[j] == ':')
                    {
                        wasSeparator = true;
                        continue;
                    }
                    if (!wasSeparator)
                    {
                        parameter.Append(currentResult[j]);
                    }
                    else
                    {
                        resultOfParameter.Append(currentResult[j]);
                    }

                }

                dataGridView5.Rows.Add(parameter, resultOfParameter);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // делаем один шаг

            Program.OneStep();



            if (Program.state == "Конец моделирования")
            {
                MessageBox.Show("Окончание моделирования");
                Program.CountResults();
                ShowResults();
                button2.Enabled = false;

                return;
            }

            //current state
            StringBuilder eventState = new StringBuilder(12);
            StringBuilder infoState  = new StringBuilder(16);
            bool wasSeparator = false;
            for (int i = 0; i < Program.state.Length; i++)
            {
                if (Program.state[i] == ':')
                {
                    wasSeparator = true;
                    continue;
                }
                if (!wasSeparator)
                {
                    eventState.Append(Program.state[i]);
                } else
                {
                    infoState.Append(Program.state[i]);
                }
                
            }
            
            dataGridView4.Rows.Add(eventState, infoState);


            // получаем информацию о системе из Program.

            // buffer
            if (Program.buffer.buffer.Count() == 0)
            {
                for (int i = 0; i < Program.bufferSize; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = "Свободен";
                }
            }
            else
            {
                for (int i = 0; i < Program.bufferSize; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i;

                    if (Program.buffer.buffer.ElementAtOrDefault(i) != null)
                        dataGridView1.Rows[i].Cells[1].Value = Program.buffer.buffer.ElementAt(i).uniqueID;
                    else
                    {
                        dataGridView1.Rows[i].Cells[1].Value = "Свободен";
                    }
                    
                }
            }

            // devices;
            if (Program.devices.Count() == 0)
            {
                for (int j = 0; j < Program.amountDevice; j++)
                {
                    dataGridView3.Rows[j].Cells[0].Value = j;
                    dataGridView3.Rows[j].Cells[1].Value = "Свободен";
                }
            } 
            else
            {
                for (int j = 0; j < Program.amountDevice; j++)
                {
                    dataGridView3.Rows[j].Cells[0].Value = j;
                    if (Program.devices.ElementAt(j).isWorking == true)
                    {
                        dataGridView3.Rows[j].Cells[1].Value = Program.devices.ElementAt(j).requestInDevice.uniqueID;
                    }
                    else
                    {
                        dataGridView3.Rows[j].Cells[1].Value = "Свободен";
                    }
                }
            }

            //sources
            for (int i = 0;  i < Program.amountSouce; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = i;
                if (Program.sources.ElementAt(i).GetTimeRequest() == -1)
                {
                    dataGridView2.Rows[i].Cells[1].Value = "Отсутствуют";
                } else
                {
                    dataGridView2.Rows[i].Cells[1].Value = Program.sources.ElementAt(i).GetTimeRequest();
                    dataGridView2.Rows[i].Cells[2].Value = Program.sources.ElementAt(i).sourceRequests.SerialNumberOfSource;
                }

               
            }

            textBox1.Text = "Время системы: " + Program.time.ToString();
            textBox2.Text = "Шаг номер: " + index.ToString();
            index++;
          
            dataGridView1.Refresh();
            dataGridView1.Update();
            dataGridView3.Refresh();
            dataGridView3.Update();         
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
