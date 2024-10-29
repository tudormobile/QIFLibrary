using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    [TestMethod]
    public void PropertyTest()
    {
        var date = DateTime.Now;
        var name = "name";
        var tick = "ABC";
        var id = "id";
        var price = 123.45m;
        var target = new Security("", "", "")
        {
            Name = name,
            UnitPrice = price,
            UnitPriceDate = date,
            Ticker = tick,
            Id = id
        };

        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(tick, target.Ticker);
        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(price, target.UnitPrice);
        Assert.AreEqual(date, target.UnitPriceDate);

    }

    [TestMethod]
    public void SecurityCollectionTest()
    {
        var target = new SecurityList();
        Assert.AreEqual(0, target.Items.Count);
    }
}