using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace ModelSystemSpecialEventMethod
{
    public partial class Results : Form
    {
        public Results()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 8;
            dataGridView1.RowCount = Program.sources.Count;

            dataGridView1.Columns[0].HeaderText = "№ И.";
            dataGridView1.Columns[1].HeaderText = "Количество заявок";
            dataGridView1.Columns[2].HeaderText = "P отк.";
            dataGridView1.Columns[3].HeaderText = "T преб.";
            dataGridView1.Columns[4].HeaderText = "Т обсл.";
            dataGridView1.Columns[5].HeaderText = "Т бп";
            dataGridView1.Columns[6].HeaderText = "D обсл";
            dataGridView1.Columns[7].HeaderText = "D бп";

            dataGridView2.ColumnCount = 2;
            dataGridView2.RowCount = Program.devices.Count;
            dataGridView2.Columns[0].HeaderText = "№ П.";
            dataGridView2.Columns[1].HeaderText = "Коэфф. использования";
            dataGridView2.Columns[1].AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void Results_Load(object sender, EventArgs e) // считается среднее..
        {
            for (int i = 0; i < Program.sources.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[1].Value = Program.sources.ElementAt(i).serialNumber;


                float Potk = (float)Program.sources.ElementAt(i).declined / Program.sources.ElementAt(i).serialNumber;
                dataGridView1.Rows[i].Cells[2].Value = Potk.ToString();


                float Tpreb = Program.sources.ElementAt(i).Tpreb / Program.sources.ElementAt(i).serialNumber;
                dataGridView1.Rows[i].Cells[3].Value = Tpreb;

                // обслуживание прибором
                float Tobcl = Program.sources.ElementAt(i).TObcl / (Program.sources.ElementAt(i).serialNumber - Program.sources.ElementAt(i).declined);
                dataGridView1.Rows[i].Cells[4].Value = Tobcl;


                // дисперсия обслуживания
                double tempSumD = 0f;
                for (int k = 0; k < Program.sources.Count - 1; k++)
                {
                    // квадрат разности
                    tempSumD += Math.Pow((Program.sources.ElementAt(i).allRequetsOfSource[k] - Program.sources.ElementAt(i).allRequetsOfSource[k+1]), 2);
                }
               
                dataGridView1.Rows[i].Cells[6].Value = (tempSumD / Program.sources.ElementAt(i).serialNumber).ToString();


                // дисперсия буферной памяти
                double tempSumDbp = 0f;
                for (int j = 0; j < Program.sources.Count - 1; j++)
                {
                    tempSumDbp = Math.Pow(Program.sources.ElementAt(i).allRequetsOfSource[j] - Program.sources.ElementAt(i).allRequetsOfSource[j+1], 2);
                }
                dataGridView1.Rows[i].Cells[7].Value = tempSumDbp / Program.sources.ElementAt(i).TbpAmount;

                float Tbp = (float)Program.sources.ElementAt(i).Tbp / Program.sources.ElementAt(i).TbpAmount;
                dataGridView1.Rows[i].Cells[5].Value = Tbp;
            }

            for (int i = 0; i < Program.devices.Count; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = i;
                float Kucp = Program.devices.ElementAt(i).sumTimeOfWork / Program.time;
                dataGridView2.Rows[i].Cells[1].Value = Kucp;
            }

        }
    }
}
