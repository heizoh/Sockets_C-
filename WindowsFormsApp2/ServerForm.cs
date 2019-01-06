using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        public Server SVR;


        private void Button1_Click(object sender, EventArgs e)
        {


            button1.Enabled = false;
            button2.Enabled = true;
            int port = 49999;
            Encoding ENC = Encoding.ASCII;
            SVR = new Server(ENC, port, textBox1);
            SVR.Connect();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SVR.DisConnect();  
        }
    }

    public class Server
    {

        public int Port;
        public Encoding ENC;
        public TextBox TB;

        //Textboxに対して直接文字列を追加すると速度低下の要因となる為、StringBuilderを用意する。
        private StringBuilder sb = new StringBuilder();

        private IPEndPoint IP;
        private TcpListener TCP;
        private TcpClient client;
        private NetworkStream NS;
        private bool Discon = false;
        private byte[] ResByte = new byte[256];
        private int ResSize = 0;
        private string ResMsg;
        private string AnsMsg;

        
        public Server(Encoding ENC, int Port, TextBox TB)
        {
            this.ENC = ENC;
            this.Port = Port;
            this.TB = TB;
            IP = new IPEndPoint(IPAddress.Any, Port);
            TCP = new TcpListener(IP);
        }

        public async void Connect()
        {

            TCP.Start();
            sb.Append("接続待ちを開始しました。\r\n");
            TB.Text = sb.ToString();
            client = await TCP.AcceptTcpClientAsync();
            NS = client.GetStream();
            MemoryStream MS = new MemoryStream();

            do
            {

                ResSize = NS.Read(ResByte, 0, ResByte.Length);
                if(ResSize == 0)
                {
                    Discon = true;
                    sb.Append("クライアントが切断しました。\r\n");
                    TB.Text = sb.ToString();
                    break;
                }

                MS.Write(ResByte, 0, ResByte.Length);
                
            } while (NS.DataAvailable || ResByte[ResSize-1] !='\n') ;

            ResMsg = ENC.GetString(MS.GetBuffer(), 0, (int)MS.Length);
            MS.Close();

            sb.Append(ResMsg + "\r\n");
            TB.Text = sb.ToString();
            
            //受信文字列に対するレスポンスを作成、返信する。
            if(!Discon)
            {

                switch (ResMsg)
                {
                    case "TEST":
                        AnsMsg = "TEST OK";
                        break;

                    default:
                        AnsMsg = "TEST NG";
                        break;
                }

                byte[] AnsByte = ENC.GetBytes(AnsMsg.ToCharArray(), 0, AnsMsg.Length);
                NS.Write(AnsByte, 0, AnsByte.Length);

                sb.Append(AnsMsg + "\r\n");
                TB.Text = sb.ToString();

            }
                        
        }

        public void DisConnect()
        {

            //通信終了の処理。
            if(NS!=null)
            {
                NS.Close();
            }
            client.Close();
            TCP.Stop();

            sb.Append("接続待ちを解除しました。\r\n");
            TB.Text = sb.ToString();

        }

    }

}
