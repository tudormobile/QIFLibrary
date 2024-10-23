using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFAccountRecordTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var name = "some name";
        var description = "some description";
        var accountType = "some type";
        var balance = 123m;
        var creditLimit = 456m;

        var target = new QIFAccountRecord(name, description, accountType, balance, creditLimit);

        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(description, target.Description);
        Assert.AreEqual(accountType, target.AccountType);
        Assert.AreEqual(balance, target.Balance);
        Assert.AreEqual(creditLimit, target.CreditLimit);

    }
}
