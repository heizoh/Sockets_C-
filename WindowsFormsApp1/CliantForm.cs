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

namespace WindowsFormsApp1
{
    public partial class CliantForm : Form
    {
        public CliantForm()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
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
            textBox3.Text = textBox3.Text + "接続先："+ipAdr+ "に\n接続しました";
        }
    }
}
