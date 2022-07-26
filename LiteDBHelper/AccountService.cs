using LiteDB;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBHelper
{
    public class AccountService
    {
        private static readonly CipherMode cipherMode = CipherMode.CBC;
        private static readonly PaddingMode paddingMode = PaddingMode.PKCS7;
        private LiteDBHelper _dbHelper;
        private string _key;
        private string _iv;

        public AccountService(string key, string iv, string? dbPwd, string? dbName = null)
        {
            if (iv.Length != 16)
            {
                throw new ArgumentException("Only 16-bit iv are accepted");
            }
            _key = key;
            _iv = iv;
            _dbHelper = new LiteDBHelper(dbPwd, dbName);
        }

        public IEnumerable<Account> GetAccounts(string category)
        {
            var accounts = _dbHelper.GetAccounts(category);
            foreach (var account in accounts)
            {
               yield return DecryptAccount(account);
            }
        }

        private Account DecryptAccount(Account account)
        {
            var decrypted = new Account();
            decrypted.Id = account.Id;
            decrypted.AccountCategory = account.AccountCategory;
            decrypted.AccountName = EncryptUtils.DecryptStringAES(account.AccountName, _key, cipherMode, _iv, paddingMode);
            decrypted.AccountValue = EncryptUtils.DecryptStringAES(account.AccountValue, _key, cipherMode, _iv, paddingMode);
            return decrypted;
        }

        private Account EncryptAccount(Account account)
        {
            var encrypted = new Account();
            encrypted.Id = account.Id;
            encrypted.AccountCategory = account.AccountCategory;
            encrypted.AccountName = EncryptUtils.EncryptStringAES(account.AccountName, _key, cipherMode, _iv, paddingMode);
            encrypted.AccountValue = EncryptUtils.EncryptStringAES(account.AccountValue, _key, cipherMode, _iv, paddingMode);
            return encrypted;
        }


        public List<string> GetCategories()
        {
            return _dbHelper.GetCategories();
        }

        public int Insert(Account account)
        {
            var encrypted = EncryptAccount(account);
            return _dbHelper.Insert(encrypted);
        }

        public bool Update(Account account)
        {
            var encrypted = EncryptAccount(account);
            return _dbHelper.Update(encrypted);
        }

        public bool Delete(Account account)
        {
            return _dbHelper.Delete(account);
        }
    }
}
