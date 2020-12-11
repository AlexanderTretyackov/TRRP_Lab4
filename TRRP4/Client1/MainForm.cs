using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        public int[,] Graph = null;
        Client _client = new Client();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (openGraph.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            try
            {
                string filename = openGraph.FileName;
                output.Text = "";
                Graph = Reader.Read(filename);
                tbFileName.Text = filename;
                btSend.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbFileName.Text = "Файл некорректен, попробуйте ещё раз";
                Graph = null;
                btSend.Enabled = false;
            }
        }

        private async void btSend_Click(object sender, EventArgs e)
        {           
            if (Graph != null)
            {
                btSend.Enabled = false;
                btnCancel.Enabled = true;
                Result result = await Task.Run(() => _client.SendSocket(Graph));
                output.Text = result.Message;
                btnCancel.Enabled = false;
                btSend.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _client.Cancel();
            btSend.Enabled = true;
        }
    }
}
