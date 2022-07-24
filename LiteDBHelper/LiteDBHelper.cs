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
        private static readonly string dbName = "ac.db";
        private string _aesKey;
        private string _dbPwd;
        private ConnectionString _connection;
        public LiteDBHelper(string aesKey, string dbPwd)
        {
            _aesKey = aesKey;
            _dbPwd = dbPwd;
            var path = Path.Combine(Environment.CurrentDirectory, dbName);
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


    }
}
