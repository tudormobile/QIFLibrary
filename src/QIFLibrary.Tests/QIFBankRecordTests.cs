using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFBankRecordTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var date = DateTime.Now;
        var amount = 1.23m;
        var memo = "some memo";
        var status = "some status";
        var address = "the payee address";
        var category = "this category";
        var description = "this is a description";
        var checkNo = "123EA";
        var target = new QIFBankRecord(
            date, amount, memo, status,
            description, category, address, checkNo);

        Assert.AreEqual(date, target.Date);
        Assert.AreEqual(amount, target.Amount);
        Assert.AreEqual(memo, target.Memo);
        Assert.AreEqual(status, target.Status);
        Assert.AreEqual(checkNo, target.CheckNo);
        Assert.AreEqual(address, target.Address);
        Assert.AreEqual(category, target.Category);
        Assert.AreEqual(description, target.Description);

        Assert.IsFalse(string.IsNullOrEmpty(target.ToString()));
    }
}
