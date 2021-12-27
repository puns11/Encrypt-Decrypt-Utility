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
        internal static List<TableUpdateModel> GetDataByTableName(BackgroundWorker backgroundWorker, string serverName, string tblName, string colName, string keyColName, string performFunction, string cryptoType, bool isBackUpRequired)
        {
            List<TableUpdateModel> tblValueList = new List<TableUpdateModel>();
            var ds = new DataSet();
            try
            {
                var dbDs = Repository.CommonMethods.LoadDbConfig();
                var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
                var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
                var connectionString = CommonMethods.GetConnectionString(sourceConfig);
                var customWhereClauseString = string.Empty;
                if(isBackUpRequired)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();

                        var sqlCmd = sqlCon.CreateCommand();
                        sqlCmd.CommandText = $"select {colName},{keyColName} into {tblName}_{DateTime.UtcNow.Ticks} from {tblName} where {colName} != ''";
                        sqlCmd.CommandType = CommandType.Text;
                        int rowsAffected = sqlCmd.ExecuteNonQuery();
                        OSILogManager.Logger.LogInfo($"Backup created successfully with table name {tblName}_{DateTime.UtcNow.Ticks}. Total rows affected: {rowsAffected.ToString()}");
                    }

                }
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    var sqlCmd = sqlCon.CreateCommand();
                    sqlCmd.CommandText = $"select {colName},{keyColName} from {tblName} where {colName} != ''";
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
                                //sqlCmd.CommandText = $"update {tblName} set {colName} = '{item[colName].ToString()}'  where {keyColName} = '{item[keyColName]}'";
                                //adpt.UpdateCommand = sqlCmd;
                                //adpt.Update(ds);
                                if (keyColName.Contains(","))
                                {
                                    var keys = keyColName.Split(',');
                                    foreach (var key in keys)
                                    {
                                        customWhereClauseString += !string.IsNullOrEmpty(customWhereClauseString) ? " and [" + key + "] = '" + item[key].ToString() + "'" : "[" + key + "] = '" + item[key].ToString() + "'";
                                    }
                                    sqlCmd.CommandText = $"update [{tblName}] set [{colName}] = '{item[colName].ToString()}'  where {customWhereClauseString}";
                                    adpt.UpdateCommand = sqlCmd;
                                    adpt.Update(ds);
                                    customWhereClauseString = string.Empty;
                                }
                                else
                                {
                                    sqlCmd.CommandText = $"update [{tblName}] set [{colName}] = '{item[colName].ToString()}'  where [{keyColName}] = '{item[keyColName]}'";
                                    adpt.UpdateCommand = sqlCmd;
                                    adpt.Update(ds);
                                }
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
                }
            }
            catch (Exception ex)
            {
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.Message}");
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to: {ex.InnerException?.Message}");

            }
            return tblValueList;
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
