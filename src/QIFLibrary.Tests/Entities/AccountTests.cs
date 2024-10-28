using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Entities
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void ConstrucorTest()
        {
            var target = new Account();
            Assert.AreEqual(String.Empty, target.AccountId);
            Assert.AreEqual(String.Empty, target.InstitutionId);
            Assert.AreEqual(Account.AccountTypes.UNKNOWN, target.AccountType);
        }

        [TestMethod]
        public void PropertyTest()
        {
            var accountId = "123";
            var institutionId = "456";
            var accountType = Account.AccountTypes.CHECKING;
            var target = new Account()
            {
                AccountType = accountType,
                InstitutionId = institutionId,
                AccountId = accountId
            };
            Assert.AreEqual(accountId, target.AccountId);
            Assert.AreEqual(institutionId, target.InstitutionId);
            Assert.AreEqual(accountType, target.AccountType);
        }

    }
}
