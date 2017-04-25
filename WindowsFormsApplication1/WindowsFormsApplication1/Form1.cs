using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        delegate void setHPCallback(int value);

        private void setHP(int val)
        {
            textBox1.Text = ""+ val;
        }

       

        Thread th = new Thread(new ThreadStart(Server.StartServer));
        public Form1()
        {
            InitializeComponent();
            th.Start();
            ServerThread.cleintConnected += new clientConnected(handleEvent);
        }

        private void handleEvent(object sender, ServerEvent e)
        {
            int val = 0;
            e.getArgs().TryGetValue("HP",out val);
            this.BeginInvoke(new setHPCallback(setHP), new object[] { val });
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

      
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Server.cmd = "stop";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
