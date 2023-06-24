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

        public override bool Equals(object? obj)
        {
            var newAc = obj as Account;
            if (newAc == null)
            {
                return false; 
            }
            if (this.AccountCategory == newAc.AccountCategory 
                && this.AccountCategory == newAc.AccountCategory
                && this.AccountName == newAc.AccountName
                && this.AccountKey == newAc.AccountKey
                && this.AccountValue == newAc.AccountValue
                )
            {
                return true;
            }
            return false;
        }
    }
}
