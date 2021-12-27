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
    public partial class DbConfigurationForm : Form
    {
        public static DataSet _ds;
        public static List<string> _authenticationTypeList;
        public event EventHandler dbConfigSaveBtn_Click;
        public DbConfigurationForm()
        {
            _ds = new DataSet();
            _authenticationTypeList = new List<string>();
            _authenticationTypeList.Add("SQL");
            _authenticationTypeList.Add("Windows");
            InitializeComponent();
        }

        private void DbConfigurationForm_Load(object sender, EventArgs e)
        {
            _ds = Repository.CommonMethods.LoadDbConfig();
            dataGridView1.DataSource = _ds;
            dataGridView1.DataMember = _ds.Tables[0].TableName;
            dataGridView1.Columns[4].Width = 160;
            ////            dataGridView1.Columns["AuthenticationType"].CellType.Name
            //DataGridViewComboBoxColumn authenCmbBoxCol = new DataGridViewComboBoxColumn();
            //authenCmbBoxCol.HeaderText = "AuthenticationType";
            //authenCmbBoxCol.Name = "AuthenticationType";
            //foreach (DataRow row in _ds.Tables[0].Rows)
            //{
            //    dataGridView1.Columns.RemoveAt(1);

            //    foreach (var item in _authenticationTypeList)
            //    {
            //        authenCmbBoxCol.Items.Add(item);
            //        if (row[1].ToString() == item)
            //        {
            //            dataGridView1.Columns.Insert(1, authenCmbBoxCol);
            //        }
            //    }
            //}



        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ds = Repository.CommonMethods.SaveDbConfig(_ds);
            if (dbConfigSaveBtn_Click != null)
                dbConfigSaveBtn_Click(this, e);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {


        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (e.Row.Index == -1)
            {
                List<Models.DbConfigModel> dbConfigModel = new List<Models.DbConfigModel>();
                dbConfigModel.Add(new Models.DbConfigModel()
                {
                    ServerName = "",
                    AuthenticationType = "",
                    DatabaseName = "",
                    Password = "",
                    UserName = ""
                });
                DataTable dt = Repository.CommonMethods.ToDataTable<Models.DbConfigModel>(dbConfigModel);
                _ds.Tables.Remove("DbConfigModel");
                _ds.Tables.Add(dt);


            }
        }
    }
}
