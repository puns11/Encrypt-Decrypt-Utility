using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt_Decrypt_Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            //string inputString = string.Empty;
            //Console.WriteLine("Enter encrypted text.");
            //inputString = Console.ReadLine();
            ////string outputString = EncryptDecrypt.decrypt(inputString);
            //string outputString = EncryptDecrypt.encrypt(inputString);
            //Console.WriteLine($"Decrypted text is: {outputString}");
            //Console.ReadLine();


            //Logic to read flat files
            try
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string filePath = settingsReader.GetValue("filePath", typeof(String)).ToString();
                string delimiter = settingsReader.GetValue("delimiter", typeof(String)).ToString();
                string performFunction = settingsReader.GetValue("performFunction", typeof(String)).ToString();
                int columnIndex = (int)settingsReader.GetValue("columnIndex", typeof(Int32));
                int skipRows = (int)settingsReader.GetValue("skipRows", typeof(Int32));
                FileInfo fileInfo = new FileInfo(filePath);
                Console.WriteLine(fileInfo.Directory);
                var outputFile = Path.Combine(fileInfo.Directory.ToString(), fileInfo.Name + "-modified" + fileInfo.Extension);
                Console.WriteLine(outputFile);
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                if (performFunction == "E" || performFunction == "D")
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
                if(ex.Message == "IncorrectPerformMethod")
                {
                    OSILogManager.Logger.LogError($"Exception on main: No Such function found to perform. There are only two types of function, for encryption enter E and for decryption enter D");
                }
                else
                {
                    OSILogManager.Logger.LogError($"Exception on main: {ex.Message}");
                    OSILogManager.Logger.LogError($"Inner Exception on main: {ex.InnerException.Message}");
                }
            }
            
        }

        private static void PerformEncryptionDecryption(string filePath, int columnIndex, int skipRows, string outputFile, string toPerform, char delimiter)
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
                        string replacedWord = toPerform == "D" ? EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.decrypt(col[columnIndex]) : EncryptDecryptTrippleDES.EncryptDecryptTrippleDES.encrypt(col[columnIndex]);
                        line = line.Replace(col[columnIndex], replacedWord);
                        output.WriteLine(line);
                    }
                    reader.Close();

                }
                output.Close();
            }
        }
    }
}
