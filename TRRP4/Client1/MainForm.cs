using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        public delegate void InvokeDelegate();
        readonly Client _client;

        public MainForm()
        {
            InitializeComponent();
            _client = new Client();
            Task.Run(() => Loading());
            var reciever = new Reciver(Client.GetLocalIP(), Configs.ClientPort);
            Task.Run(() => reciever.BeginRecieve());
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var answer = _client.StartWorking();
                output.BeginInvoke(new InvokeDelegate(
                () => { output.Text = $"Полученный ответ: {answer}"; }));
            });

            //btSend.Enabled = false;
            //btnCancel.Enabled = true;
            //Result result = await Task.Run(() => _client.SendSocket(Graph));
            //output.Text = result.Message;
            //btnCancel.Enabled = false;
            //btSend.Enabled = true;
        }

        private void Loading()
        {
            while (!Client.loaded)
            {
                Thread.Sleep(1000);
            }
            output.BeginInvoke(new InvokeDelegate(
                () => { output.Text = $"Загружено, найдено клиентов в сети {Client.otherClients.Count}"; }));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _client.Cancel();
            btSend.Enabled = true;
        }

    }
}
