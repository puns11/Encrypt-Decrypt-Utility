using Crypto_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_UI.Repository
{
    public class CommonMethods
    {
        public static DataSet LoadDbConfig()
        {
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\DbConfiguration.xml");
            }
            catch (Exception ex)
            {

                OSILogManager.Logger.LogError($"LoadDbConfig method failed due to :{ex.Message}");
                OSILogManager.Logger.LogError($"LoadDbConfig method failed due to :{ex.InnerException?.Message}");
            }
            return ds;
        }

        public static DataSet SaveDbConfig(DataSet ds)
        {
            try
            {
                ds.WriteXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\DataSource\DbConfiguration.xml"));
            }
            catch (Exception ex)
            {

                OSILogManager.Logger.LogError($"LoadDbConfig method failed due to :{ex.Message}");
                OSILogManager.Logger.LogError($"LoadDbConfig method failed due to :{ex.InnerException?.Message}");
            }
            return ds;
        }

        internal static DataSet GetDataByTableName(string columnName, string tableName, string serverName)
        {
            var viewDataSet = new DataSet();
            try
            {
                var dbDs = Repository.CommonMethods.LoadDbConfig();
                var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
                var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
                var connectionString = GetConnectionString(sourceConfig);
                if (sourceConfig != null)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var sqlCmd = connection.CreateCommand();
                        sqlCmd.CommandText = $"select top 50 {columnName} from {tableName}";
                        sqlCmd.CommandType = CommandType.Text;
                        var adpt = new SqlDataAdapter(sqlCmd);
                        adpt.Fill(viewDataSet);
                    }

                }
            }
            catch (Exception ex)
            {
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to : {ex.Message}");
                OSILogManager.Logger.LogError($"GetDataByTableName method failed due to : {ex.InnerException?.Message}");
            }
            return viewDataSet;
        }

        /// <summary>
        /// Convert the List<T> into DataTable.
        /// </summary>
        /// <param name="List<T> items">List of <T>type</param>
        /// <returns>It returns DataTable from the given list</returns>       
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        Type t = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                        pro.SetValue(obj, dr[column.ColumnName] == System.DBNull.Value ? GetValue(column.DataType) : Convert.ChangeType(dr[column.ColumnName], t), null);
                    }

                    else
                        continue;
                }
            }
            return obj;
        }

        private static object GetValue(Type dataType)
        {
            if (dataType.Equals(typeof(float)) || dataType.Equals(typeof(double)) || dataType.Equals(typeof(decimal)) || dataType.Equals(typeof(byte)) || dataType.Equals(typeof(int)))
            {
                return null;
            }

            else if (dataType.Equals(typeof(string)))
            {
                return string.Empty;
            }

            else if (dataType.Equals(typeof(char)))
            {
                return Char.MinValue;
            }
            else if (dataType.Equals(typeof(DateTime)))
            {
                return (DateTime?)null;
            }
            else if (dataType.Equals(typeof(bool)))
            {
                return (bool?)null;
            }
            else
            {
                return string.Empty;
            }

        }

        public static Dictionary<string, string> GetDataType<T>(DataTable dt)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataColumn column in dt.Rows[0].Table.Columns)
            {
                var columnName = column.ToString();
                var dataType = column.DataType.Name;
                dict.Add(columnName, dataType);
            }
            return dict;
        }

        internal static bool ConnectDb(DbConfigModel dbConfigModel)
        {
            bool isConnected = false;
            try
            {
                var connectionString = GetConnectionString(dbConfigModel);
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    OSILogManager.Logger.LogInfo("SQLDataAdapter: Connection created with server: " + dbConfigModel.ServerName);

                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                        OSILogManager.Logger.LogInfo("SQLDataAdapter: Connection Successful");
                        isConnected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                OSILogManager.Logger.LogError($"ConnectDb method failed due to : {ex.Message}");
                OSILogManager.Logger.LogError($"ConnectDb method failed due to : {ex.InnerException?.Message}");

            }
            return isConnected;
        }

        public static string GetConnectionString(DbConfigModel dbConfigModel)
        {
            string connectionString = string.Empty;
            if (dbConfigModel.AuthenticationType != "SQL")
            {
                connectionString = $"Server={dbConfigModel.ServerName};Database={dbConfigModel.DatabaseName};Integrated Security=true;";
            }
            else if (dbConfigModel.AuthenticationType == "SQL")
            {
                connectionString = $"Server={dbConfigModel.ServerName};Database={dbConfigModel.DatabaseName};User Id={dbConfigModel.UserName};Password={dbConfigModel.Password};";
            }
            else
            {
                OSILogManager.Logger.LogInfo("Unable to connect");
                return "";
            }
            return connectionString;
        }

        public static Dictionary<string, List<TableContainer>> GetSourceTables(string serverName, string tableName)
        {

            Dictionary<string, List<TableContainer>> dict = new Dictionary<string, List<TableContainer>>();
            var colList = new List<TableContainer>();
            var primaryKeys = string.Empty;
            var dbDs = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(dbDs.Tables[0]);
            var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
            var connectionString = GetConnectionString(sourceConfig);
            if (sourceConfig != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");

                    // List<string> TableNames = new List<string>();
                    //foreach (DataRow row in schema.Rows)
                    //{
                    //    if (!string.Equals("sysdiagrams", row[2].ToString(), StringComparison.InvariantCultureIgnoreCase))
                    //    {
                    // TableNames.Add(row[2].ToString());
                    var ds = new DataSet();
                    var sqlCmd = connection.CreateCommand();
                    sqlCmd.CommandText = $"SELECT c.name as columnName,t.name as datatype FROM sys.columns c inner join (select * from sys.types) t on c.system_type_id = t.system_type_id WHERE object_id = OBJECT_ID('{tableName}') order by c.name";  // No data wanted, only schema
                    sqlCmd.CommandType = CommandType.Text;

                    var adpt = new SqlDataAdapter(sqlCmd);
                    adpt.Fill(ds);

                    foreach (DataRow col in ds.Tables[0].Rows)
                    {
                        colList.Add(new TableContainer { ColumnName = col.Field<string>("ColumnName"), DataType = col.Field<string>("datatype") });
                        // list.Add(new TableContainer { ColumnName = col.Field<string>("ColumnName"), DataType = row.Field<string>("datatype") });
                    }
                    //fetch primary keys for given table
                    sqlCmd.CommandText = $@"
                                            select schema_name(tab.schema_id) as [schema_name], 
                                                col.[name] as column_name, 
                                                tab.[name] as table_name
                                            from sys.tables tab
                                                left
                                            join sys.indexes pk

                                            on tab.object_id = pk.object_id

                                            left
                                            join sys.index_columns ic

                                            on ic.object_id = pk.object_id

                                            and ic.index_id = pk.index_id
                                                left join sys.columns col
                                                    on pk.object_id = col.object_id
                                                    and col.column_id = ic.column_id

                                                where tab.name = '{tableName}' and isnull(pk.is_primary_key,0) = 1
                                            order by schema_name(tab.schema_id),
                                                pk.[name],
                                                ic.index_column_id";


                    adpt.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            primaryKeys += primaryKeys != "" ? "," + item["column_name"].ToString() : item["column_name"].ToString();
                        }
                        foreach (var item in colList)
                        {
                            item.KeyColumn = primaryKeys;
                        }
                    }
                    //    }
                    //}
                }

            }
            dict.Add(primaryKeys, colList);
            return dict;
        }

        internal static List<string> GetTables(string serverName, BackgroundWorker backgroundWorker)
        {

            List<string> tblList = new List<string>();
            var ds = Repository.CommonMethods.LoadDbConfig();
            var dbConfigModel = Repository.CommonMethods.ConvertDataTable<Models.DbConfigModel>(ds.Tables[0]);
            var sourceConfig = dbConfigModel.Find(x => x.ServerName == serverName);
            var connectionString = GetConnectionString(sourceConfig);
            try
            {
                if (sourceConfig != null)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schema = connection.GetSchema("Tables");

                        // List<string> TableNames = new List<string>();
                        //foreach (DataRow row in schema.Rows)
                        //{
                        //if (!string.Equals("sysdiagrams", row[2].ToString(), StringComparison.InvariantCultureIgnoreCase))
                        //{
                        DataSet tbDS = new DataSet();
                        var sqlCmd = connection.CreateCommand();
                        sqlCmd.CommandText = $"select name from sys.tables order by name";
                        sqlCmd.CommandType = CommandType.Text;

                        var adpt = new SqlDataAdapter(sqlCmd);
                        adpt.Fill(tbDS);
                        float rowCount = 0;
                        var totalCount = ds.Tables[0].Rows.Count;
                        foreach (DataRow item in tbDS.Tables[0].Rows)
                        {
                            var intPercentage = (int)Math.Round((rowCount / totalCount) * 100, 0);
                            backgroundWorker.ReportProgress(intPercentage);
                            tblList.Add(item[0].ToString());
                        }
                        //}
                        //}
                        // return TableNames.OrderBy(t => t).ToList();
                    }

                }
            }
            catch (Exception ex)
            {

                OSILogManager.Logger.LogError($"GetTables method failed due to : {ex.Message}");
                OSILogManager.Logger.LogError($"GetTables method failed due to : {ex.InnerException?.Message}");

            }
            if (tblList.Count == 0)
            {
                tblList.Add("No Table found.");
            }
            return tblList;
        }
    }
}
