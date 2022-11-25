using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto_UI
{
    public partial class ScanConfigurationForm : Form
    {
        public static DataSet _ds;
        public static List<string> _authenticationTypeList;
        public event EventHandler scanDbConfigSaveBtn_Click;
        public ScanConfigurationForm()
        {
            _ds = new DataSet();
            _authenticationTypeList = new List<string>();
            _authenticationTypeList.Add("SQL");
            _authenticationTypeList.Add("Windows");
            InitializeComponent();
        }

        private void DbConfigurationForm_Load(object sender, EventArgs e)
        {
            _ds = Repository.CommonMethods.LoadScanConfig();
            dataGridView1.DataSource = _ds;
            dataGridView1.DataMember = _ds.Tables[0].TableName;
            //dataGridView1.Columns[4].Width = 160;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void scanDbSaveBtn_Click(object sender, EventArgs e)
        {
            _ds = Repository.CommonMethods.SaveScanConfig(_ds);
            if (scanDbConfigSaveBtn_Click != null)
                scanDbConfigSaveBtn_Click(this, e);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {


        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (e.Row.Index == -1)
            {
                List<Models.ScanConfigModel> scanConfigModel = new List<Models.ScanConfigModel>();
                scanConfigModel.Add(new Models.ScanConfigModel()
                {
                    TableName = "",
                    ColumnName = "",
                    Algorithm = ""
                });
                DataTable dt = Repository.CommonMethods.ToDataTable<Models.ScanConfigModel>(scanConfigModel);
                _ds.Tables.Remove("ScanConfigModel");
                _ds.Tables.Add(dt);


            }
        }
    }
}
