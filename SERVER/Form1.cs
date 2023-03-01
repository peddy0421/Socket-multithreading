using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SERVER
{
    public partial class Form1 : Form
    {
        TcpListener server = null;
        TcpClient clientconnection;
        NetworkStream Datastream;
        public Form1()
        {
            InitializeComponent();
        }
//=========================================================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            IPAddress localAddr = IPAddress.Parse("192.168.1.100");
            server = new TcpListener(localAddr, 8000); //Opens server port
            server.Start(); //Starts server
            timer1.Start(); //Starts timer to check stream at regular intervals
         }
//=========================================================================================

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                clientconnection = server.AcceptTcpClient(); //Accepts connection request
                try
                {
                    while (true)
                    {
                        Datastream = clientconnection.GetStream(); //Gets the data stream
                        byte[] buffer = new byte[clientconnection.ReceiveBufferSize]; //Gets required buffer size
                        int Data = Datastream.Read(buffer, 0, clientconnection.ReceiveBufferSize); //Gets message (encoded)
                        string message = Encoding.ASCII.GetString(buffer, 0, Data); //Decoodes message
                        myUI(message, textBox1);
                      
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("CLIENT Disconnect!!");
                    clientconnection.Close();
                }
            });
        }
//=========================================================================================
        private delegate void myUICallBack(string myStr, Control ctl);
        private void myUI(string myStr, Control ctl)
        {
            if (this.InvokeRequired)
            {
                myUICallBack myUpdate = new myUICallBack(myUI);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                ctl.Text = myStr;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes("False");
            Datastream.Write(msg, 0, msg.Length);
        }
    }
}
