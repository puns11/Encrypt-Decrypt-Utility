using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto_UI
{
    public partial class ApplicationLogForm : Form
    {
        public string _logFilePath { get; set; }
        public ApplicationLogForm()
        {
            InitializeComponent();
            _logFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Encrypt-Decrypt.log";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplicationLogForm_Load(object sender, EventArgs e)
        {
            appLogRichTextBox.Text = "";


            using (TextReader reader = File.OpenText(_logFilePath))
            {
                appLogRichTextBox.Text = reader.ReadToEnd();
            }    

        }

        private void appLogCloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void clearBtn_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to clear the application logs?", "Delete Logs", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question))
            {
                appLogRichTextBox.Text = string.Empty;
                await File.WriteAllTextAsync(_logFilePath, string.Empty);
            }
            
        }
    }
}
