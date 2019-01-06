using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private static void Main()
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {

            string SendMsg;
            if (textBox3.Text == null || textBox3.Text.Length == 0)
            {
                return;
            }
            else
            {
                SendMsg = textBox3.Text;
            }

            string ipAdr;

            if (textBox2.Text == "")
            {
                ipAdr = "127.0.0.1";
            }
            else
            {
                ipAdr = textBox2.Text;
            }

            TcpClient tcp = new TcpClient(ipAdr, 49999);
            textBox4.Text = textBox4.Text + "接続先："+ipAdr+ "に\n接続しました";

            NetworkStream NS = tcp.GetStream();

            NS.ReadTimeout = 100000;
            NS.WriteTimeout = 100000;

            Encoding Enc = Encoding.ASCII;
            byte[] SendByte = Enc.GetBytes(SendMsg +'\n');

            NS.Write(SendByte, 0, SendByte.Length);

            System.IO.MemoryStream MS = new System.IO.MemoryStream();
            byte[] ResByte = new byte[256];
            int ResSize = 0;

            do
            {

                ResSize = NS.Read(ResByte, 0, ResByte.Length);
                if (ResSize == 0)
                {
                    textBox4.Text = textBox4.Text + "サーバーが切断しました。\n";
                    break;
                }
                MS.Write(ResByte, 0, ResSize);

            } while (NS.DataAvailable || ResByte[ResSize - 1] != '\n');
            string ResMsg = Enc.GetString(MS.GetBuffer(), 0, (int)MS.Length);
            MS.Close();

            textBox4.Text = textBox4.Text + ResMsg+ '\n';

            NS.Close();
            tcp.Close();

            textBox4.Text = textBox4.Text + "切断しました。\n";

        }
    }
}
