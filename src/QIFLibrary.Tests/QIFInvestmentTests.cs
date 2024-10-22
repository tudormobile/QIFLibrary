using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFInvestmentTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var date = DateTime.Now;
        var amount = 1.23m;
        var memo = "some memo";
        var status = "some status";
        var check = "check status";
        var payee = "the payee";
        var address = "the payee address";
        var category = "this category";
        var securityName = "security name";
        var price = 2.34m;
        var quantity = 1234;
        var commission = 3.45m;
        var splitAmount = 4.56m;
        var target = new QIFInvestment(date, amount, memo, status, check, payee, address, category, QIFInvestmentType.ShrsOut, securityName, price, quantity, commission, splitAmount);

        Assert.AreEqual(date, target.Date);
        Assert.AreEqual(amount, target.Amount);
        Assert.AreEqual(memo, target.Memo);
        Assert.AreEqual(status, target.Status);
        Assert.AreEqual(check, target.Check);
        Assert.AreEqual(target.Payee, payee);
        Assert.AreEqual(address, target.Address);
        Assert.AreEqual(category, target.Category);
        Assert.AreEqual(securityName, target.SecurityName);
        Assert.AreEqual(commission, target.Commision);
        Assert.AreEqual(splitAmount, target.SplitAmount);
        Assert.AreEqual(quantity, target.Quantity);
        Assert.AreEqual(category, target.Category);
        Assert.AreEqual(price, target.Price);
        Assert.AreEqual(QIFInvestmentType.ShrsOut, target.InvestmentAction);

        Assert.IsFalse(string.IsNullOrEmpty(target.ToString()));
    }
}
