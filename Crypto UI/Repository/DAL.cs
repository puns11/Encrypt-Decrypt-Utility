using Crypto_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_UI.Repository
{
    public class DAL
    {
        internal static int GetDataByTableName(BackgroundWorker backgroundWorker, string serverName, string tblName, string colName, string keyColName, string performFunction, string cryptoType, bool isBackUpRequired)
        {
            var rowsAffected = 0;
            var ds = new DataSet();
            try
            {
                var dbDs = Repository.CommonMethods.LoadDbConfig();
                var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
                var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
                var connectionString = CommonMethods.GetConnectionString(sourceConfig);
                var customWhereClauseString = string.Empty;
                if (isBackUpRequired)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();

                        var sqlCmd = sqlCon.CreateCommand();
                        sqlCmd.CommandText = $"select {colName},{keyColName} into {tblName}_{DateTime.UtcNow.Ticks} from {tblName} where {colName} != ''";
                        sqlCmd.CommandType = CommandType.Text;
                        var rowsBackedUp = sqlCmd.ExecuteNonQuery();
                        OSILogManager.Logger.LogInfo($"Backup created successfully with table name {tblName}_{DateTime.UtcNow.Ticks}. Total rows affected: {rowsBackedUp.ToString()}");
                    }

                }
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    var sqlCmd = sqlCon.CreateCommand();
                    sqlCmd.CommandText = $"select {keyColName},{colName} from {tblName} where {colName} != ''";
                    sqlCmd.CommandType = CommandType.Text;

                    var adpt = new SqlDataAdapter(sqlCmd);
                    adpt.Fill(ds);
                    using (CryptoController.CryptoFactory factory = new CryptoController.CryptoFactory())
                    {
                        float rowCount = 0;
                        var totalCount = ds.Tables[0].Rows.Count;

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            var intPercentage = (int)Math.Round((rowCount / totalCount) * 100, 0);
                            backgroundWorker.ReportProgress(intPercentage);
                            var originalValue = item[colName].ToString();
                            try
                            {
                                var adapter = factory.CreateCryptoAdapter(cryptoType);
                                var replacedValue = performFunction == "Decrypt" ? adapter.Decrypt(item[colName].ToString()) : adapter.Encrypt(item[colName].ToString());

                                item[colName] = replacedValue;
                            }
                            catch (Exception exc)
                            {
                                OSILogManager.Logger.LogError($"Rows iteration encryption/decryption failed due to: {exc.Message}");
                                OSILogManager.Logger.LogError($"Rows iteration encryption/decryption failed due to: {exc.InnerException?.Message}");
                            }
                            rowCount += 1;
                        }
                        rowCount = 0;
                    }
                    var colDetailList = new List<ColumnDetail>();

                    DataTable schema = sqlCon.GetSchema("Columns");
                    foreach (System.Data.DataRow sch in schema.Rows)
                    {
                        if (sch.ItemArray[2].ToString() == tblName && sch.ItemArray[3].ToString() == colName)
                        {
                            colDetailList.Add(new ColumnDetail
                            {
                                ColumnLength = sch.ItemArray[8].ToString(),
                                ColumnName = sch.ItemArray[3].ToString(),
                                ColumnType = sch.ItemArray[7].ToString()

                            });
                        }
                        if (keyColName.Contains(","))
                        {
                            var keys = keyColName.Split(',');
                            foreach (var key in keys)
                            {
                                if (sch.ItemArray[2].ToString() == tblName && sch.ItemArray[3].ToString() == key)
                                {
                                    colDetailList.Add(new ColumnDetail
                                    {
                                        ColumnLength = sch.ItemArray[8].ToString(),
                                        ColumnName = sch.ItemArray[3].ToString(),
                                        ColumnType = sch.ItemArray[7].ToString()

                                    });
                                }
                            }

                        }
                        else
                        {
                            if (sch.ItemArray[2].ToString() == tblName && sch.ItemArray[3].ToString() == keyColName)
                            {
                                colDetailList.Add(new ColumnDetail
                                {
                                    ColumnLength = sch.ItemArray[8].ToString(),
                                    ColumnName = sch.ItemArray[3].ToString(),
                                    ColumnType = sch.ItemArray[7].ToString()

                                });
                            }
                        }
                    }


                    string createTblStr = $"CREATE TABLE #TmpTbl{tblName}(";
                    string createColStr = string.Empty;

                    foreach (var colItem in colDetailList)
                    {
                        createColStr += string.IsNullOrEmpty(createColStr) ? colItem.ColumnName + " " + (colItem.ColumnType.ToLower().Contains("char") ? colItem.ColumnType + $"({colItem.ColumnLength})" : colItem.ColumnType) : "," + colItem.ColumnName + " " + (colItem.ColumnType.Contains("char") ? colItem.ColumnType + $"({colItem.ColumnLength})" : colItem.ColumnType);
                    }

                    createTblStr = createTblStr + createColStr + ")";
                    sqlCmd.CommandText = createTblStr;

                    sqlCmd.ExecuteNonQuery();

                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlCon))
                    {
                        bulkcopy.BulkCopyTimeout = 600;
                        bulkcopy.DestinationTableName = $"#TmpTbl{tblName}";
                        bulkcopy.WriteToServer(ds.Tables[0]);
                        bulkcopy.Close();
                    }

                    if (keyColName.Contains(","))
                    {
                        var keys = keyColName.Split(',');
                        foreach (var key in keys)
                        {
                            customWhereClauseString += !string.IsNullOrEmpty(customWhereClauseString) ? " and T.[" + key + "] = M.[" + key + "] " : "T.[" + key + "] = M.[" + key + "] ";
                        }
                        sqlCmd.CommandText = $"update M set [{colName}] = T.[{colName}] from [{tblName}] M inner join [#TmpTbl{tblName}] T on  {customWhereClauseString} where T.[{colName}] != M.[{colName}]; DROP TABLE [#TmpTbl{tblName}];";
                        customWhereClauseString = string.Empty;
                    }
                    else
                    {
                        sqlCmd.CommandText = $"update M set [{colName}] = T.[{colName}] from [{tblName}] M inner join [#TmpTbl{tblName}] T on T.[{keyColName}] = M.[{keyColName}]  where T.[{colName}] != M.[{colName}]; DROP TABLE [#TmpTbl{tblName}];";                        
                    }
                    
                    rowsAffected = sqlCmd.ExecuteNonQuery();
                    OSILogManager.Logger.LogInfo($"Total rows affected after update:{rowsAffected.ToString()}");
                }
            }
            catch (Exception ex)
            {
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.Message}");
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.InnerException?.Message}");

            }
            return rowsAffected;
        }

        internal static bool VerifyPII(string serverName, string tableName, string columnName, string keyColName, string cryptoType, BackgroundWorker backgroundWorker)
        {
            var isPIIVerified = true;
            var ds = new DataSet();
            var dbDs = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
            var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
            var connectionString = CommonMethods.GetConnectionString(sourceConfig);
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                var sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandText = $"select {keyColName},{columnName} from {tableName} where {columnName} != ''";
                sqlCmd.CommandType = CommandType.Text;

                var adpt = new SqlDataAdapter(sqlCmd);
                adpt.Fill(ds);
                using (CryptoController.CryptoFactory factory = new CryptoController.CryptoFactory())
                {
                    float rowCount = 0;
                    var totalCount = ds.Tables[0].Rows.Count;
                    StringBuilder errorLogStr = new StringBuilder();
                    using(var adapter = factory.CreateCryptoAdapter(cryptoType))
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            var intPercentage = (int)Math.Round((rowCount / totalCount) * 100, 0);
                            backgroundWorker.ReportProgress(intPercentage);
                            var originalValue = item[columnName].ToString();

                            try
                            {
                                var replacedValue = adapter.Decrypt(item[columnName].ToString());
                            }
                            catch (Exception exc)
                            {
                                string colNameValues = string.Empty;

                                if (keyColName.Contains(","))
                                {
                                    var keys = keyColName.Split(',');

                                    foreach (var key in keys)
                                    {
                                        if (string.IsNullOrEmpty(colNameValues))
                                        {
                                            colNameValues = key + $"='{item[key]}'";
                                        }
                                        else
                                        {
                                            colNameValues += " and " + key + $"='{item[key]}'";
                                        }
                                    }
                                }
                                else
                                {
                                    colNameValues = keyColName + $"='{item[keyColName]}'";
                                }

                                errorLogStr.AppendLine($"Data not encrypted for primary key {colNameValues} due to error : {exc.Message}");
                                isPIIVerified = false;
                            }
                            rowCount += 1;
                        }
                    }
                    
                    if (!isPIIVerified)
                    {
                        OSILogManager.Logger.LogError(errorLogStr.ToString());
                    }
                    rowCount = 0;
                }
            }
            return isPIIVerified;
        }

        //internal static void UpdateColNameByTableName(string serverName, string tblName, string colName, string colValue)
        //{
        //    var ds = new DataSet();
        //    try
        //    {
        //        var dbDs = Repository.CommonMethods.LoadDbConfig();
        //        var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
        //        var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
        //        var connectionString = CommonMethods.GetConnectionString(sourceConfig);
        //        using (SqlConnection sqlCon = new SqlConnection(connectionString))
        //        {
        //            sqlCon.Open();

        //            var sqlCmd = sqlCon.CreateCommand();
        //            sqlCmd.CommandText = $"update  {tblName} set {colName} = {colValue} from {tblName}";
        //            sqlCmd.CommandType = CommandType.Text;

        //            var adpt = new SqlDataAdapter(sqlCmd);
        //            adpt.Fill(ds);
        //            foreach (DataRow item in ds.Tables[0].Rows)
        //            {
        //                tblValueList.Add(new TableUpdateModel()
        //                {
        //                    OriginalValue = item[colName].ToString(),
        //                    UpdatedValue = null
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.Message}");
        //        OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.InnerException?.Message}");

        //    }
        //}
    }
}
