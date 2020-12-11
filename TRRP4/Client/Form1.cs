using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private readonly Client _client = new Client();

        public Form1()
        {
            InitializeComponent();
        }

        private async void StartNormalize_Click(object sender, EventArgs e)
        {
            ChangeUI(true);
            var success = int.TryParse(TbNumber.Text, out int number);
            if (success && number >= 0 && number < 2000000)
            {
                var result = await Task.Run(() => _client.SendSocket(TbNumber.Text));
                LbAnswer.Text = result.Message;
                if (!BtCancel.Visible)
                    LbAnswer.Text = "Действие отменено";
            }
            else
            {
                LbAnswer.Text = "Please enter an integer not a negative number less 2 000 000";
            }
            ChangeUI(false);
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            ChangeUI(false);
            _client.Cancel();
        }

        private void ChangeUI(bool load)
        {
            StartNormalize.Text = load ? 
                "Loading...":
                "Узнать!";
            StartNormalize.Enabled = !load;
            BtCancel.Visible = load;
        }
    }
}
