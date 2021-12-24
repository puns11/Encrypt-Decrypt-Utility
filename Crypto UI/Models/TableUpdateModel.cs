using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_UI.Models
{
    public class TableUpdateModel
    {
        public int PrimaryKey { get; set; }
        public string OriginalValue { get; set; }
        public string UpdatedValue { get; set; }

    }
}
