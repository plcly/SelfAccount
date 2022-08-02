using LiteDBHelper;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEncryptUtils()
        {
            var text = "1234";

            var key = "1234";
            var s = EncryptUtils.EncryptStringAES(text, key);
            Assert.AreEqual("501defc4013f3f21529c123f33c065ad", s.ToLower());

            var d = EncryptUtils.DecryptStringAES(s, key);
            Assert.AreEqual(text, d);

            key = "1234567890123456789012345678901234567890";
            s = EncryptUtils.EncryptStringAES(text, key);
            Assert.AreEqual("3153c5fd8f556200c915b10eb8178e64", s.ToLower());
            d = EncryptUtils.DecryptStringAES(s, key);
            Assert.AreEqual(text, d);

            key = "asdf1234qwer1234asdf1234";
            s = EncryptUtils.EncryptStringAES(text, key, System.Security.Cryptography.CipherMode.CBC);
            Assert.AreEqual("fde63c64e3070c042f3aab3fdfc8a9cc", s.ToLower());
            d = EncryptUtils.DecryptStringAES(s, key, System.Security.Cryptography.CipherMode.CBC);
            Assert.AreEqual(text, d);

            key = "asdf1234qwer1234asdf1234";
            s = EncryptUtils.EncryptStringAES(text, key, System.Security.Cryptography.CipherMode.CBC, "1234123412341234");
            Assert.AreEqual("0d9a7115c63fea859cb225644202c16f", s.ToLower());
            d = EncryptUtils.DecryptStringAES(s, key, System.Security.Cryptography.CipherMode.CBC, "1234123412341234");
            Assert.AreEqual(text, d);
        }

        [TestMethod]
        public void TestAccountService()
        {
            if (File.Exists("UnitTest.db"))
            {
                File.Delete("UnitTest.db");
            }
            var service = new AccountService("1234", "1234567890abcdef", null, "UnitTest.db");
            service.Insert(new Account
            {
                AccountCategory = "test1",
                AccountName = "a@a.a",
                AccountValue = "asdf1234"
            });
            service.Insert(new Account
            {
                AccountCategory = "test2",
                AccountName = "b@b.b",
                AccountValue = "1234asdf"
            });

            var categories=service.GetCategories().ToList();
            Assert.AreEqual(2, categories.Count);
            Assert.IsTrue(categories.Any(p=>p== "test1"));
            Assert.IsTrue(categories.Any(p=>p== "test2"));

            var accounts1 = service.GetAccounts("test1").ToList();
            Assert.AreEqual(1, accounts1.Count);
            Assert.AreEqual("a@a.a", accounts1.First().AccountName);
            Assert.AreEqual("asdf1234", accounts1.First().AccountValue);

            var accounts2 = service.GetAccounts("test2").ToList();
            Assert.AreEqual(1, accounts2.Count);
            Assert.AreEqual("b@b.b", accounts2.First().AccountName);
            Assert.AreEqual("1234asdf", accounts2.First().AccountValue);

            var searchAccounts = service.SearchAccounts("a").ToList();
            Assert.AreEqual(1, searchAccounts.Count);
            Assert.AreEqual("a@a.a", accounts1.First().AccountName);

            var d1 = service.Delete(accounts1.First());
            var d2 = service.Delete(accounts2.First());
            Assert.AreEqual(true, d1);
            Assert.AreEqual(true, d2);

            var categoriesDeleted = service.GetCategories();
            Assert.AreEqual(0, categoriesDeleted.Count());
        }
    }
}