using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_UI.Models
{
    public class DbConfigModel
    {

        public string DisplayName { get; set; }

        public string ServerName { get; set; }

        public string AuthenticationType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DatabaseName { get; set; }
    }

    public class ScanConfigModel
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string KeyColName { get; set; }

        public string Algorithm { get; set; }
    }

}
