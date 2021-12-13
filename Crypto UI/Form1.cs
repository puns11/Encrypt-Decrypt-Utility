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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                Filter = "txt files (*.txt)|*.txt",
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
                int columnIndex = Convert.ToInt32(colIndexTxtBox.Text);
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
                    PerformEncryptionDecryption(filePath, columnIndex, skipRows, outputFile, performFunction,delimiter.ToCharArray()[0]);
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
        private static void PerformEncryptionDecryption(string filePath, int columnIndex, int skipRows, string outputFile, string toPerform,char delimiter)
        {
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
                        string[] col = line.Split(delimiter);
                        string replacedWord = toPerform == "Decrypt" ? EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.decrypt(col[columnIndex]) : EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.encrypt(col[columnIndex]);
                        line = line.Replace(col[columnIndex], replacedWord);
                        output.WriteLine(line);
                    }
                    reader.Close();

                }
                output.Close();
            }
        }

        private void sbmitOnTxtBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string toPerform = selectFnOnTxtCmBox.Text;
                outputTxtBox.Text = toPerform == "Decrypt" ? EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.decrypt(enterTxtBox.Text) : EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.encrypt(enterTxtBox.Text);
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

        }
    }
}
