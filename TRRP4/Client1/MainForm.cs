using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        Client _client = new Client();
        public MainForm()
        {
            InitializeComponent();
            Loading();
        }

        private async void btSend_Click(object sender, EventArgs e)
        {
            //btSend.Enabled = false;
            //btnCancel.Enabled = true;
            //Result result = await Task.Run(() => _client.SendSocket(Graph));
            //output.Text = result.Message;
            //btnCancel.Enabled = false;
            //btSend.Enabled = true;
        }

        private async void Loading()
        {
            while (!Client.loaded)
            {
                Thread.Sleep(1000);
            }
            output.Text = "Loaded";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _client.Cancel();
            btSend.Enabled = true;
        }
    }
}
