using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
/*
 * The problem of creating another class for receving and sending data
 * is not worth it since we can't really change form1 assests without complex code
 * It's simply not worth it and its also not the most efficient way of doing it.
 */
namespace Guldkortet
{
    public partial class Form1 : Form
    {
        int port = 12345;
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            label1.BackColor = System.Drawing.Color.Transparent;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * Det som görs att man utför filladning genom en annan CPU tråd
             * Detta ökar hastigheten och låser inte applikationen under tiden.
             */
            button1.Enabled = false;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            Thread.Sleep(1000);
            button2.Enabled = true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Most of what is being written here is design, its made to update some 
            //sort of bar or label.
            int sum = 0;
            backgroundWorker1.ReportProgress(sum);
            Fileloader loader = new Fileloader();
            sum = 15;
            backgroundWorker1.ReportProgress(sum);
            sum = 65;
            loader.UserLoader("kundlista.txt");
            backgroundWorker1.ReportProgress(sum);
            loader.CardLoader("kortlista.txt");
            sum = 95;
            backgroundWorker1.ReportProgress(sum);
            sum = 100;
            backgroundWorker1.ReportProgress(sum);
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker1.ReportProgress(0);
                return;
            }
            e.Result = sum;
            Thread.Sleep(10);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Denna metod kallas varje gång man kör ReportProgress.
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage.ToString() + "%";
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //När allt är klar finns det tre alternativ.
            if (e.Cancelled)
            {
                label1.Text = "Operation Terminated";
            }
            else if (e.Error != null)
            {
                label1.Text = e.Error.ToString();
            }
            else
            {
                label1.Text = "100% Completed!";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            InfoHandle.Instance.StartEverything();
            button2.BackColor = Color.Green;
            button2.Enabled = false;
        } 
        //We are listening on any IP, but port 12345
        //Here we are just saying that we accepting requests.
        //Creating a recursion so it keeps reading the information.
        //And since its async its fine it will be running on another thread.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void progressBar1_Click(object sender, EventArgs e)
        {
        }
    }
}