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
using System.Diagnostics;

namespace RealTimeCharts
{
    public partial class Form1 : Form
    {
        private Thread cpuThread;
        private double[] cpuArray = new double[60];

        public Form1()
        {
            InitializeComponent();
            //By : RifqiArief
        }

        private void getPerformance()
        {
            var cpuPerfCounter = new PerformanceCounter("Processor information", "% Processor Time", "_Total");

            while (true)
            {
                cpuArray[cpuArray.Length - 1] = Math.Round(cpuPerfCounter.NextValue(), 0);
                Array.Copy(cpuArray, 1, cpuArray, 0, cpuArray.Length - 1);
                try
                {
                    if (chartCPU.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate { UpdateChartCPU(); });
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Something Wrong /n Restart Application ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            Application.Restart();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }

        private void UpdateChartCPU()
        {
            chartCPU.Series["Series1"].Points.Clear();

            for (int i = 0; i < cpuArray.Length; i++)
            {
                chartCPU.Series["Series1"].Points.AddY(cpuArray[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cpuThread = new Thread(new ThreadStart(getPerformance));
            cpuThread.IsBackground = true;
            cpuThread.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTanggal.Text = DateTime.Now.ToString("dd-MM-yyyy");
            labelWaktu.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelTanggal.Text = DateTime.Now.ToString("dd-MM-yyyy");
            labelWaktu.Text = DateTime.Now.ToString("HH:mm:ss");
            timer1.Start();
        }
    }
}
