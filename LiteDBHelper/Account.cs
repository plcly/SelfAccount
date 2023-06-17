using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBHelper
{
    public class BaseModel
    {
        public int Id { get; set; }
    }
    public class Account: BaseModel
    {
        public string AccountCategory { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string AccountValue { get; set; }
    }
}
