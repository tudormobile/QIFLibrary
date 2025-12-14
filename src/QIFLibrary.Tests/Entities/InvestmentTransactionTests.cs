using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Entities;

[TestClass]
public class InvestmentTransactionTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var id = "123";
        var sid = "MSFT";
        var type = InvestmentTransactionType.BUYSTOCK;
        var target = new InvestmentTransaction(id, sid, type);

        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(sid, target.SecurityId);
        Assert.AreEqual(type, target.TransactionType);

        // defaults:
        Assert.AreEqual(0m, target.UnitPrice, "Default value must be zero.");
        Assert.AreEqual(0m, target.Units, "Default value must be zero.");
        Assert.AreEqual(0m, target.Total, "Default value must be zero.");
        Assert.AreEqual(0m, target.CommissionAndFees, "Default value must be zero.");
        Assert.AreEqual(DateTime.Now.Month, target.DatePosted.Month, "Default should be [now-ish]");
        Assert.AreEqual(DateTime.Now.Day, target.DatePosted.Day, "Default should be [now-ish]");
        Assert.AreEqual(DateTime.Now.Year, target.DatePosted.Year, "Default should be [now-ish]");
    }

    [TestMethod]
    public void ConstructorTest2()
    {
        var id = "id";
        var sid = "MSFT";
        var type = InvestmentTransactionType.BUYSTOCK;
        var units = 123.456m;
        var unitPrice = 33.4455m;
        var total = units * unitPrice;
        var date = DateTime.Now.AddDays(-10);
        var commission = 10m;
        var target = new InvestmentTransaction(id, sid, type, date, units, unitPrice, total + commission);

        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(sid, target.SecurityId);
        Assert.AreEqual(type, target.TransactionType);

        // defaults:
        Assert.AreEqual(unitPrice, target.UnitPrice);
        Assert.AreEqual(units, target.Units);
        Assert.AreEqual(total + commission, target.Total);
        Assert.AreEqual(commission, target.CommissionAndFees);
        Assert.AreEqual(date, target.DatePosted);
    }

    [TestMethod]
    public void InvestmentTransactionCollectionTest()
    {
        var start = DateTime.Now;
        var end = DateTime.Now.AddDays(1);
        var target = new InvestmentTransactionList()
        {
            Start = start,
            End = end
        };

        Assert.IsEmpty(target.Items);
        Assert.AreEqual(start, target.Start);
        Assert.AreEqual(end, target.End);
    }


}