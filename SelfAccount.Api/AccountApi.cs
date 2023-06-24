using LiteDBHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SelfAccount.Api
{
    public class AccountApi
    {
        public static string Test()
        {
            return "Hello World" + DateTime.Now;
        }

        public static async Task<string> GetAll(string key, string? iv)
        {
            if (string.IsNullOrWhiteSpace(iv))
            {
                iv = Config.IV;
            }
            var service = new AccountService(key, iv, Config.DBPwd, Config.DBName);
            var allAccounts = service.GetAllAccounts();

            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, allAccounts);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        public static async Task<bool> PushAll([FromBody] PushModel model)
        {
            if (string.IsNullOrWhiteSpace(model.IV))
            {
                model.IV = Config.IV;
            }
            var service = new AccountService(model.Key, model.IV, Config.DBPwd, Config.DBName);
            var allAccounts = service.GetAllAccounts();

            await using var stream = new MemoryStream();
            var newAccounts = JsonSerializer.Deserialize<List<Account>>(model.Json);
            foreach (var ac in newAccounts)
            {
                if (allAccounts.Any(p => p.Equals(ac)))
                {
                    continue;
                }
                var clone = new Account
                {
                    AccountCategory = ac.AccountCategory,
                    AccountName = ac.AccountName,
                    AccountKey = ac.AccountKey,
                    AccountValue = ac.AccountValue,
                };
                service.Insert(clone);
            }
            return true;
        }

        
    }
    public class PushModel
    {
        public string Key { get; set; }
        public string? IV { get; set; }
        public string Json { get; set; }
    }
}
