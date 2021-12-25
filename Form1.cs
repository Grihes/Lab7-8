using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Lab7
{
    public partial class Form1 : Form
    {
        double p;
        public Form1()
        {
            InitializeComponent();
        }
        void fillData(double p)
        {
            int l = 22;
            bool[,] array = Program.Percol(p,l);
            int[,] arrayLabels = Program.SetLabels(array);
            Dictionary<int, int> clustersNumberS = Program.FindClustersNumberS(array);
            textBox3.Text = "Label of unit clusters is " + Program.FindUnitClustersLabel(array).ToString();
            textBox4.Text = "Average Size of cluster is " + Program.FindAverageClusterSize(array).ToString();
            textBox5.Text = "Probability is " + Program.FindProbability(array, l);
            for (int i = 0; i < arrayLabels.GetLength(0); i++)
            {
                for (int j = 0; j < arrayLabels.GetLength(1); j++)
                    Console.Write("{0}  ", arrayLabels[i, j]);
                Console.WriteLine();
            }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    Console.Write("{0}  ", array[i, j]);
                Console.WriteLine();
            }
            int maxRow = array.GetLength(0);
            int maxCol = array.GetLength(1);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;

            int rowHeight = dataGridView1.ClientSize.Height / maxRow - 1;
            int colWidth = dataGridView1.ClientSize.Width / maxCol - 1;

            for (int i = 0; i < maxRow; i++) dataGridView1.Columns.Add(i.ToString(), "");
            for (int i = 0; i < maxRow; i++) dataGridView1.Columns[i].Width = colWidth;
            dataGridView1.Rows.Add(maxRow);
            for (int j = 0; j < maxRow; j++) dataGridView1.Rows[j].Height = rowHeight;

            for (int j = 0; j < maxRow; j++)
            {
                for (int i = 0; i < maxCol; i++)
                {
                    if (array[j, i])
                        dataGridView1[i, j].Style.BackColor = Color.Red;
                    else
                        dataGridView1[i, j].Style.BackColor = Color.Black;
                }
            }
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    if (arrayLabels[i, j]!=0)
                        dataGridView1[j,i].Value = arrayLabels[i,j];
                }
            }

            int t = 0;
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(clustersNumberS.Count());
            foreach (var element in clustersNumberS)
            {
                dataGridView2[0, t].Value = element.Key;
                dataGridView2[1, t].Value = element.Value;
                t++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             p = double.Parse(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            textBox2.Text = "p= " + p.ToString();
            fillData(p);
        }



        private void button3_Click_1(object sender, EventArgs e)
        {
            p += 0.025;
        }
    }
}
