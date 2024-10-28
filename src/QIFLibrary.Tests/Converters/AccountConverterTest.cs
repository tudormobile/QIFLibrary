using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Converters;

[TestClass]
public class AccountConverterTest
{
    [TestMethod]
    public void ConvertBankAccountTest()
    {
        var institutionId = "iid";
        var accountId = "123";
        var accountType = OFXAccountType.CHECKING;

        var root = new OFXProperty("BANKACCTTO");

        root.Children.Add(new OFXProperty("ACCTID", accountId));
        root.Children.Add(new OFXProperty("BANKID", institutionId));
        root.Children.Add(new OFXProperty("ACCTTYPE", accountType.ToString()));

        var target = new AccountConverter();
        var actual = target.Convert(root);

        Assert.IsNotNull(actual);
        Assert.AreEqual(institutionId, actual.InstitutionId);
        Assert.AreEqual(accountId, actual.AccountId);
        Assert.AreEqual((int)accountType, (int)actual.AccountType);
    }

    [TestMethod]
    public void ConvertInvestmentAccountTest()
    {
        var institutionId = "iid";
        var accountId = "123";

        var root = new OFXProperty("INVACCTFROM");
        root.Children.Add(new OFXProperty("ACCTID", accountId));
        root.Children.Add(new OFXProperty("BROKERID", institutionId));

        var target = new AccountConverter();
        var actual = target.Convert(root);

        Assert.IsNotNull(actual);
        Assert.AreEqual(institutionId, actual.InstitutionId);
        Assert.AreEqual(accountId, actual.AccountId);
        Assert.AreEqual(Account.AccountTypes.INVESTMENT, actual.AccountType);
    }

    [TestMethod]
    public void ConverterTest()
    {
        var root = new OFXProperty("Bad Root");

        var target = new OFXPropertyConverter();
        var actual = target.GetAccount(root);

        Assert.IsNull(actual);
    }

}
