using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Thread th = new Thread(new ThreadStart(Server.StartServer));
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            th.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Server.cmd = "stop";
        }
    }
}
