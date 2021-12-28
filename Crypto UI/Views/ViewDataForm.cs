using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto_UI.Views
{
    public partial class ViewDataForm : Form
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string ServerName { get; set; }
        public ViewDataForm()
        {
            InitializeComponent();
        }

        private void ViewDataForm_Load(object sender, EventArgs e)
        {
            var ds = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(ds.Tables[0]);
            var sourceConfig = dbConfigModel.Find(x => x.ServerName == ServerName);

            var viewDataSet = Repository.CommonMethods.GetDataByTableName(ColumnName, TableName, ServerName);
            viewDataGridView.DataSource = viewDataSet;
            viewDataGridView.DataMember = viewDataSet.Tables[0].TableName;
        }
    }
}
