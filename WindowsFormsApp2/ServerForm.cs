using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            System.Net.IPEndPoint iP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 39999);
            TcpListener tcp = new TcpListener(iP);
            tcp.Start();
            TcpClient Cliant = tcp.AcceptTcpClient();
            textBox1.Text = textBox1.Text + "接続されました。";
            NetworkStream ns = Cliant.GetStream();
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            Encoding Enc = Encoding.ASCII;

            bool Discon = false;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] ResByte = new byte[256];
            int ResSize = 0;

            do
            {
                ResSize = ns.Read(ResByte, 0, ResByte.Length);
                if (ResSize == 0)
                {
                    Discon = true;
                    textBox1.Text = textBox1.Text + "クライアントが切断しました。\n";
                    break;
                }
                ms.Write(ResByte, 0, ResByte.Length);

            } while (ns.DataAvailable || ResByte[ResSize - 1] != '\n');

            string ResMsg = Enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            textBox1.Text = textBox1.Text + ResMsg + '\n';

            if (!Discon)
            {
                string SendMsg = ResMsg.Length.ToString();
                byte[] SendByte = Enc.GetBytes(SendMsg + '\n');

                ns.Write(SendByte, 0, SendByte.Length);
                textBox1.Text = textBox1.Text + SendMsg + '\n';
            }

            ns.Close();
            Cliant.Close();
            textBox1.Text = textBox1.Text +　"クライアントとの接続を閉じました\n";

            tcp.Stop();
            textBox1.Text = textBox1.Text + "受信待機終了\n";

        }
    }

    public class Server
    {

    }

}
