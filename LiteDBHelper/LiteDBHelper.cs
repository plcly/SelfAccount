using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBHelper
{
    public class LiteDBHelper
    {
        private string _dbName = "ac.db";
        private ConnectionString _connection;
        public LiteDBHelper(string? dbPwd, string? dbName = null)
        {
            if (dbName!=null)
            {
                _dbName = dbName; 
            }
            var path = Path.Combine(Environment.CurrentDirectory, _dbName);
            _connection = new ConnectionString
            {
                Filename = path,
                Password = dbPwd,
            };
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();
            }
        }

        public IEnumerable<Account> GetAccounts(string category)
        {
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();
                return col.Find(p => p.AccountCategory == category).ToList();
            }
        }

        public IEnumerable<string> GetCategories()
        {
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();

                return col.FindAll().Select(p => p.AccountCategory).ToList();
            }
        }

        public int Insert(Account account)
        {
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();
                if (!col.Exists(p => p.AccountCategory == account.AccountCategory && p.AccountName == account.AccountName))
                {
                    return col.Insert(account);
                }
                return 0;
            }
        }

        public bool Update(Account account)
        {
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();
                if (col.Exists(p => p.Id == account.Id))
                {
                    return col.Update(account);
                }
                return false;
            }
        }

        public bool Delete(Account account)
        {
            using (var db = new LiteDatabase(_connection))
            {
                var col = db.GetCollection<Account>();
                if (col.Exists(p => p.Id == account.Id))
                {
                    return col.Delete(account.Id);
                }
                return false;
            }
        }
    }
}
