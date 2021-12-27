﻿using Crypto_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto_UI
{
    public partial class HomeForm : Form
    {
        public string dbBasedSelFnComboBoxText { get; set; }
        public string envComboBoxText { get; set; }
        public string tblNameComboBoxText { get; set; }
        public string colNameComboBoxText { get; set; }
        public bool dbBasedTrippleDesRadBtnChecked { get; set; }
        public string dbBasedAesRadBtnText { get; set; }
        public string dbBasedTrippleDesRadBtnText { get; set; }
        public string keyColNameText { get; set; }
        public bool isBackRequired { get; set; }
        public List<string> tblList { get; set; }
        public HomeForm()
        {
            InitializeComponent();
            tblList = new List<string>();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;


            sqlConBackGroundWorker.WorkerReportsProgress = true;
            sqlConBackGroundWorker.DoWork += new DoWorkEventHandler(sqlConBackgroundWorker_DoWork);
            sqlConBackGroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            sqlConBackGroundWorker.RunWorkerCompleted += sqlConBackgroundWorker_RunWorkerCompleted;

        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = true;
            }
            toolStripProgressBar1.Visible = false;
        }
        private void sqlConBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = true;
            }
            tblNameComboBox.Enabled = true;
            //populate table names
            tblNameComboBox.Items.Clear();
            foreach (var item in tblList)
            {
                tblNameComboBox.Items.Add(item);
            }
            colNameComboBox.Enabled = true;
            toolStripProgressBar1.Visible = false;
        }

        private void dbBasedSaveBtn_Click(object sender, EventArgs e)
        {
            dbBasedTrippleDesRadBtnChecked = dbBasedTrippleDesRadBtn.Checked;
            dbBasedTrippleDesRadBtnText = dbBasedTrippleDesRadBtn.Text;
            dbBasedAesRadBtnText = dbBasedAesRadBtn.Text;
            colNameComboBoxText = colNameComboBox.Text;
            tblNameComboBoxText = tblNameComboBox.Text;
            envComboBoxText = envComboBox.Text;
            dbBasedSelFnComboBoxText = dbBasedSelFnComboBox.Text;
            dbBasedTrippleDesRadBtnText = dbBasedTrippleDesRadBtn.Text;
            keyColNameText = colNameComboBox.SelectedValue.ToString();
            toolStripProgressBar1.Visible = true;
            isBackRequired = isBkUpReqCheckBox.Checked;
            foreach (Control item in this.Controls)
            {
                item.Enabled = false;
            }
            backgroundWorker.RunWorkerAsync();
            
        }
        void sqlConBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var ds = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(ds.Tables[0]);
            if (Repository.CommonMethods.ConnectDb(dbConfigModel.Find(x => x.ServerName == envComboBoxText)))
            {
                MessageBox.Show("Connection Successful");
                tblList = Repository.CommonMethods.GetTables(envComboBoxText,sqlConBackGroundWorker);
            }
            else
            {
                MessageBox.Show($"Unable to connect to Server: {envComboBoxText}");
            }
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(dbBasedSelFnComboBoxText) && !string.IsNullOrEmpty(envComboBoxText) && !string.IsNullOrEmpty(tblNameComboBoxText) && !string.IsNullOrEmpty(colNameComboBoxText))
            {
                
                string cryptoType = dbBasedTrippleDesRadBtnChecked ? dbBasedTrippleDesRadBtnText : dbBasedAesRadBtnText;
                string performFunction = dbBasedSelFnComboBoxText;
                string tblName = tblNameComboBoxText;
                string colName = colNameComboBoxText;
                string keyColName = keyColNameText;
                string serverName = envComboBoxText;
                bool isBackUpRequired = isBackRequired;
                if (string.IsNullOrEmpty(keyColName))
                {
                    MessageBox.Show("No primary key found for this table, update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (performFunction == "Encrypt" || performFunction == "Decrypt")
                {
                    PerformDbEncryptionDecryption(serverName, tblName, colName, keyColName, performFunction, cryptoType, isBackUpRequired);
                }
                else
                {
                    throw new Exception("IncorrectPerformMethod");
                }
                MessageBox.Show("Changes completed.");

            }
            else
            {
                MessageBox.Show("Please required fields.");
                return;
            }
        }
        
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt|csv files(*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileNameTxtBox.Text = openFileDialog1.FileName;
                rowsSkipTxtBox.Enabled = true;
                delimiterTxtBox.Enabled = true;
                colIndexTxtBox.Enabled = true;
                sbFIleBtn.Enabled = true;

                //default values
                rowsSkipTxtBox.Text = "1";
                delimiterTxtBox.Text = "|";
                colIndexTxtBox.Text = "0";

            }
        }

        private void sbFIleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string filePath = fileNameTxtBox.Text;
                string delimiter = delimiterTxtBox.Text;
                string performFunction = selectFnOnFileCmBox.Text;
                string cryptoType = trippleDesRadioBtn.Checked ? trippleDesRadioBtn.Text : aesRadioBtn.Text;
                List<int> colNumbers = new List<int>();
                if (colIndexTxtBox.Text.Contains(","))
                {
                    var colNumberInput = colIndexTxtBox.Text.Split(',');
                    foreach (var item in colNumberInput)
                    {
                        colNumbers.Add(Convert.ToInt32(item));
                    }
                }
                else
                {
                    colNumbers.Append(Convert.ToInt32(colIndexTxtBox.Text));
                }
                int skipRows = Convert.ToInt32(rowsSkipTxtBox.Text);
                FileInfo fileInfo = new FileInfo(filePath);
                Console.WriteLine(fileInfo.Directory);
                var outputFile = Path.Combine(fileInfo.Directory.ToString(), fileInfo.Name + "-modified" + fileInfo.Extension);
                Console.WriteLine(outputFile);
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                if (performFunction == "Encrypt" || performFunction == "Decrypt")
                {
                    PerformEncryptionDecryption(filePath, colNumbers, skipRows, outputFile, performFunction,delimiter.ToCharArray()[0], cryptoType);
                }
                else
                {
                    throw new Exception("IncorrectPerformMethod");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "IncorrectPerformMethod")
                {
                    OSILogManager.Logger.LogError($"Exception on main: No Such function found to perform. There are only two types of function, for encryption enter E and for decryption enter D");
                    MessageBox.Show("An exception occured. Please review logs for details.");
                }
                else
                {
                    OSILogManager.Logger.LogError($"Exception on main: {ex.Message}");
                    OSILogManager.Logger.LogError($"Inner Exception on main: {ex.InnerException?.Message}");
                    MessageBox.Show("An exception occured. Please review logs for details.");
                }
            }
            finally
            {
                MessageBox.Show("Process Completed");
            }
        }
        private static void PerformEncryptionDecryption(string filePath, List<int> columnIndex, int skipRows, string outputFile, string toPerform,char delimiter, string cryptoType)
        {
            using (CryptoController.CryptoFactory factory = new CryptoController.CryptoFactory())
            {
                var adapter = factory.CreateCryptoAdapter(cryptoType);
                using (StreamWriter output = new StreamWriter(outputFile, true))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        if (skipRows > 0)
                        {
                            for (int i = 0; i < skipRows; i++)
                            {
                                line = reader.ReadLine();
                                output.WriteLine(line);
                            }
                        }
                        
                        while ((line = reader.ReadLine()) != null)
                        {
                            try
                            {
                                string[] col = line.Split(delimiter);
                                foreach (int colIndex in columnIndex)
                                {
                                    string replacedWord = toPerform == "Decrypt" ? adapter.Decrypt(col[colIndex]) : adapter.Encrypt(col[colIndex]);
                                    line = line.Replace(col[colIndex].Trim(), replacedWord);
                                }
                                
                                output.WriteLine(line);
                            }
                            catch (Exception ex)
                            {
                                OSILogManager.Logger.LogError($"PerformEncryptionDecryption method failed: {ex.Message}");
                                
                            }
                        }
                        reader.Close();

                    }
                    output.Close();
                }
            }
                
        }

        private void sbmitOnTxtBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string toPerform = selectFnOnTxtCmBox.Text;
                string cryptoType = trippleDesTxtRadioBtn.Checked ? trippleDesTxtRadioBtn.Text : aesTxtRadioBtn.Text;
                using (CryptoController.CryptoFactory factory = new CryptoController.CryptoFactory())
                {
                    var adapter = factory.CreateCryptoAdapter(cryptoType);
                    outputTxtBox.Text = toPerform == "Decrypt" ? adapter.Decrypt(enterTxtBox.Text) : adapter.Encrypt(enterTxtBox.Text);
                }
                    
            }
            catch (Exception ex)
            {
                if (ex.Message == "IncorrectPerformMethod")
                {
                    OSILogManager.Logger.LogError($"Exception on main: No Such function found to perform. There are only two types of function, for encryption enter E and for decryption enter D");
                    MessageBox.Show("An exception occured. Please review logs for details.");
                }
                else
                {
                    OSILogManager.Logger.LogError($"Exception on main: {ex.Message}");
                    OSILogManager.Logger.LogError($"Inner Exception on main: {ex.InnerException?.Message}");
                    MessageBox.Show("An exception occured. Please review logs for details.");
                }

            }
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            selectFnOnTxtCmBox.SelectedText = "Encrypt";
            LoadEnvList();

            //envComboBox.DisplayMember = ds.Tables[0].Columns["ServerName"].ToString();
            //envComboBox.ValueMember = ds.Tables[0].Columns["UserName"].ToString();

        }

        private void LoadEnvList()
        {
            DataSet ds = new DataSet();
            ds = Repository.CommonMethods.LoadDbConfig();
            List<Models.DbConfigModel> configModel = new List<Models.DbConfigModel>();
            configModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(ds.Tables[0]);
            envComboBox.DataSource = new BindingSource(ds.Tables[0], null);
            envComboBox.DisplayMember = ds.Tables[0].Columns[0].ColumnName;
            envComboBox.DisplayMember = ds.Tables[0].Columns[0].ColumnName;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void dbConfigSaveBtn_Click(object sender, EventArgs e)
        {
            LoadEnvList();
        }
        private void signAsBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*.*",
                Filter = "All files (*.*)|*.*|asc files (*.asc)|*.asc",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                signAsTxtBox.Text = openFileDialog1.FileName;
                outputFIleTxtBox.Enabled = true;
            }
        }

        private void signAsChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if(signAsChkBox.Checked)
            {
                signAsTxtBox.Enabled = true;
                signAsBrowseBtn.Enabled = true;
            }
            else
            {
                signAsTxtBox.Enabled = false;
                signAsBrowseBtn.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*.*",
                Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|csv files(*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                inputFileTxtBox.Text = openFileDialog1.FileName;
                signGroupBox.Enabled = true;
                outputFIleTxtBox.Text = openFileDialog1.FileName + ".gpg";

            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DbConfigurationForm form1 = new DbConfigurationForm();
            form1.dbConfigSaveBtn_Click += new EventHandler(dbConfigSaveBtn_Click);
            form1.WindowState = FormWindowState.Normal;
            form1.Show();
            form1.StartPosition = FormStartPosition.CenterScreen;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void databaseBasedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dbTabpage.Show();
            tabControl.Focus();
            dbTabpage.Focus();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            envComboBoxText = envComboBox.Text;
            toolStripProgressBar1.Visible = true;
            
            foreach (Control item in this.Controls)
            {
                item.Enabled = false;
            }
            sqlConBackGroundWorker.RunWorkerAsync();
        }

        private void tblNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tblNameComboBox.Text != null)
            {
                string tblName = tblNameComboBox.Text;
                RefreshColumnCombobox(tblName);


            }
        }

        private void RefreshColumnCombobox(string tblName)
        {
            colNameComboBox.DataSource = null;
            dbBasedSaveBtn.Enabled = false;
            var ds = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(ds.Tables[0]);
            if (Repository.CommonMethods.ConnectDb(dbConfigModel.Find(x => x.ServerName == envComboBox.Text)))
            {
                //populate table names
                Dictionary<string,List<TableContainer>> colDict = new Dictionary<string, List<TableContainer>>();
                colDict = Repository.CommonMethods.GetSourceTables(envComboBox.Text, tblName);
                foreach (var item in colDict)
                {
                    
                    foreach (var colList in item.Value)
                    {
                        colNameComboBox.DataSource = item.Value;
                        colNameComboBox.DisplayMember = "ColumnName";
                        colNameComboBox.ValueMember = "KeyColumn";
                        //.Items.Add(colList.ColumnName);
                    }

                }
                
                colNameComboBox.Enabled = true;
                isBkUpReqCheckBox.Enabled = true;
                dbBasedSaveBtn.Enabled = true;

            }
            else
            {
                MessageBox.Show($"Unable to connect to Server: {envComboBox.Text}");
            }
        }


        private void PerformDbEncryptionDecryption(string serverName, string tblName, string colName, string keyColName, string performFunction, string cryptoType, bool isBackUpRequired)
        {
            using (CryptoController.CryptoFactory factory = new CryptoController.CryptoFactory())
            {
                var adapter = factory.CreateCryptoAdapter(cryptoType);
                List<TableUpdateModel> tblList = new List<TableUpdateModel>();
                tblList = Repository.DAL.GetDataByTableName(backgroundWorker, serverName, tblName, colName, keyColName, performFunction, cryptoType, isBackUpRequired);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationLogForm alForm = new ApplicationLogForm();
            alForm.Show();
        }

        private void applicationLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm abForm = new AboutForm();
            abForm.Show();
        }
    }
}
