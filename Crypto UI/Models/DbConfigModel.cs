using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_UI.Models
{
    public class DbConfigModel
    {
        public string ServerName { get; set; }

        public string AuthenticationType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DatabaseName { get; set; }
    }
}
