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
                    PerformEncryptionDecryption(filePath, columnIndex, skipRows, outputFile, performFunction);
                }
                else
                {
                    Console.WriteLine("No function is performed.");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            
        }

        private static void PerformEncryptionDecryption(string filePath, int columnIndex, int skipRows, string outputFile, string toPerform)
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
                        string[] col = line.Split('|');
                        string replacedWord = toPerform == "D" ? EncryptDecrypt.decrypt(col[columnIndex]) : EncryptDecrypt.encrypt(col[columnIndex]);
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
