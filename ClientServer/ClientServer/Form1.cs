using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ClientServer
{
    public partial class Form1 : Form
    {
        
        Socket S;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            S.Disconnect(false);
            S.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                S.Connect(IPAddress.Parse(richTextBox3.Text), Convert.ToInt32(richTextBox4.Text));
                MessageBox.Show("You're Connected");
            }
            
            catch (Exception)
            {
                MessageBox.Show("Error. Something went wrong. Try again (:");

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                S.Send(Encoding.UTF8.GetBytes(richTextBox2.Text));
            }

            catch(ObjectDisposedException)
            {
                MessageBox.Show("Cant send while you're not connected");
            }
            
            catch (NullReferenceException)
            {
                MessageBox.Show("Cant send while you're not connected");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        int Getport()
        {
            int result = 13337;

            this.Invoke((MethodInvoker)delegate() { result = Convert.ToInt32(richTextBox5.Text); } );
            return result;
        }

        public void Tcplisten()
        {
            TcpListener server;
            int port;
            port = Getport();
            server = new TcpListener(port);
            server.Start();

            Byte[] bytes = new Byte[256];
            String data = null;

            while(true)
            {
                TcpClient client = server.AcceptTcpClient();
                MessageBox.Show("A client has connected");

                data = null;

                NetworkStream stream = client.GetStream();

                int i;
                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a UTF8 string.
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        this.Invoke((MethodInvoker)delegate () { richTextBox1.Text += data + "\n" ; });

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

                    }
                }

                
                catch (InvalidOperationException)
                {
                    MessageBox.Show("The client has disconnected");
                    client.Close();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("The client has disconnected");
                    client.Close();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread Ttcp = new Thread(Tcplisten);
            Ttcp.Start();
        }
    }
}   

