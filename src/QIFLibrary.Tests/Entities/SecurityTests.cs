using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Entities;

[TestClass]
public class SecurityTests
{
    [TestMethod]
    public void SecurityTest()
    {
        var date = DateTime.Now;
        var name = "name";
        var tick = "ABC";
        var id = "id";
        var price = 123.45m;
        var target = new Security(id, tick, name, price, date);

        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(tick, target.Ticker);
        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(price, target.UnitPrice);
        Assert.AreEqual(date, target.UnitPriceDate);
        Assert.AreEqual(Security.SecurityTypes.UNKNOWN, target.SecurityType);
        Assert.AreEqual(Security.SecurityIdTypes.OTHER, target.SecurityIdType);
    }

    [TestMethod]
    public void PropertyTest()
    {
        var date = DateTime.Now;
        var name = "name";
        var tick = "ABC";
        var id = "id";
        var price = 123.45m;
        var sectype = Security.SecurityTypes.MUTUALFUND;
        var secIdType = Security.SecurityIdTypes.CUSIP;
        var target = new Security("", "", "")
        {
            Name = name,
            UnitPrice = price,
            UnitPriceDate = date,
            Ticker = tick,
            Id = id,
            SecurityType = sectype,
            SecurityIdType = secIdType
        };

        Assert.AreEqual(sectype, target.SecurityType);
        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(tick, target.Ticker);
        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(price, target.UnitPrice);
        Assert.AreEqual(date, target.UnitPriceDate);
        Assert.AreEqual(secIdType, target.SecurityIdType);
    }

    [TestMethod]
    public void SecurityCollectionTest()
    {
        var target = new SecurityList();
        Assert.IsEmpty(target.Items);
    }
}