using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Collatz_Conjecture
{
    //O: 3x+1
    //E: x/2

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GraphSetup();
        }
        long[] digits;
        long currentState = 0;
        Thread thread;
        List<int> loops;

        //------------ BUTTONS --------------//

        private void button1_Click(object sender, EventArgs e)
        {
            long max = Convert.ToInt64(textBox1.Text);
            if (max > 0)
                CalculateSequence(max);
            else
                MessageBox.Show("Insert a number greater than zero!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            long max = Convert.ToInt64(textBox1.Text);
            if (max > 0)
            {
                InitializeGraph();
                thread = new Thread(() => CalculateFirstDigitDistribution(max));
                thread.Start();
                timer1.Start();
                progressBar1.Maximum = (int)max+1;
            }
            else
                MessageBox.Show("Insert a number greater than zero!");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            long max = Convert.ToInt64(textBox1.Text);
            if (max > 0)
            {
                InitializeGraph();
                /*thread = new Thread(() => CalculateLoopCountDistribution(max));
                thread.Start();*/
                CalculateLoopCountDistribution(max);
                timer1.Start();
                progressBar1.Maximum = (int)max + 1;
            }
            else
                MessageBox.Show("Insert a number greater than zero!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitializeGraph();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
                thread.Abort();
        }

        //------------ TIMERS --------------//


        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDigitChart();
            textBox2.Text = string.Join(", ", digits);
            progressBar1.Value = (int)currentState;
            if (!thread.IsAlive)
                timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateLoopChart();
            progressBar1.Value = (int)currentState;
            if (!thread.IsAlive)
                timer1.Stop();
        }

        //------------ FUNCTIONS --------------//

        private void CalculateLoopCountDistribution(long max)
        {
            loops = new List<int>();

            for (long k = 1; k <= max; k++)
            {
                currentState = k;
                long i = k;
                int l = 0;
                do
                {
                    l++;
                    i = (i % 2 == 0) ? i / 2 : i * 3 + 1;
                } while (i != 1);
                
                
            }
        }

        private void CalculateFirstDigitDistribution(long max)
        {
            digits = new long[] {0,0,0,0,0,0,0,0,0};

            for (long k = 1; k <= max; k++)
            {
                currentState = k;
                long i=k;
                digits[(int)(i.ToString()[0]) - 48 - 1]++;
                do
                {
                    i = (i % 2 == 0) ? i/2 : i * 3 + 1;
                    digits[(int)(i.ToString()[0]) - 48 - 1]++;
                } while (i != 1);
            }
        }

        private void CalculateSequence(long max)
        {
            InitializeGraph();
            string sequence = max.ToString() + "; ";
            chart1.Series[0].Points.AddY(max);
            chart1.Series[1].Points.AddY(max);
            do
            {
                max = (max % 2 == 0) ? max / 2 : max * 3 + 1;
                sequence += max.ToString() + "; ";
                chart1.Series[0].Points.AddY(max);
                chart1.Series[1].Points.AddY(max);
            } while (max != 1);
            textBox2.Text = sequence;
        }

        private void InitializeGraph()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            textBox2.Text = "";
            progressBar1.Maximum = 0;
            currentState = 0;
        }

        private void GraphSetup()
        {
            chart1.Series[0].IsValueShownAsLabel = true;
            chart1.ChartAreas["ChartArea1"].BackColor = Color.FromArgb(0, 3, 38);
            chart1.ChartAreas["ChartArea1"].AxisX.LineColor = Color.FromArgb(120, 255, 255, 255);
            chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.FromArgb(120, 255, 255, 255);
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = Color.FromArgb(120, 255, 255, 255);
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = Color.FromArgb(120, 255, 255, 255);
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        }

        private void UpdateDigitChart()
        {
            chart1.Series[2].Points.Clear();
            for (int i = 0; i < digits.Length; i++)
            {
                chart1.Series[2].Points.AddXY(i + 1, digits[i]);
            }
        }

        private void UpdateLoopChart()
        {
            /*chart1.Series[2].Points.Clear();
            for (int i = 0; i < digits.Length; i++)
            {
                chart1.Series[2].Points.AddXY(i + 1, digits[i]);
            }*/


            textBox2.Text = loops.ToString();
        }
    }
}
