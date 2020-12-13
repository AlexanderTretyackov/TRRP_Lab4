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
        readonly Reciver reciever;
        public static MainForm CurrentForm;
        public MainForm()
        {
            InitializeComponent();
            CurrentForm = this;
            _client = new Client();
            Task.Run(() => Loading());
            reciever = new Reciver(Client.GetLocalIP(), Configs.ClientPort);
            Reciver.Form = this;
            Task.Run(() => reciever.BeginRecieve());
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                _client.DistributeWork();
                //string strAnswer = "Не удалось получить ответ на задачу";
                //if (answer != null)
                //    strAnswer = $"Полученный ответ: {answer.Data.A}";
                //output.BeginInvoke(new InvokeDelegate(
                //() => { output.Text = strAnswer; }));
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
                () =>
                {
                    output.Text = $"Загружено, найдено клиентов в сети {Client.otherClients.Count}";
                    output.Visible = info.Visible = true;
                    lbLoading.Visible = false;
                }));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _client.Cancel();
            btSend.Enabled = true;
        }

    }
}
