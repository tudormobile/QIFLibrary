using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Entities;

[TestClass]
public class PositionTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var id = Guid.NewGuid().ToString();
        var target = new Position(id);

        Assert.AreEqual(id, target.SecurityId);

        Assert.AreEqual(0m, target.UnitPrice);
        Assert.AreEqual(0m, target.Units);
        Assert.AreEqual(0m, target.MarketValue);

        Assert.AreEqual(String.Empty, target.Memo);
        Assert.AreEqual(default, target.PriceDate);
        Assert.AreEqual(Position.PositionTypes.UNKNOWN, target.PositionType);
        Assert.AreEqual(Position.PositionAccountTypes.OTHER, target.SubAccountType);
        Assert.AreEqual(Security.SecurityTypes.UNKNOWN, target.SecurityType);

    }

    [TestMethod]
    public void PropertyTest()
    {
        var id = Guid.NewGuid().ToString();
        var unitPrice = 123.45m;
        var units = 678.9m;
        var priceDate = DateTime.Now;
        var memo = "this is the memo.";
        var positionType = Position.PositionTypes.SHORT;
        var subAccountType = Position.PositionAccountTypes.CASH;
        var securityType = Security.SecurityTypes.MUTUALFUND;
        var marketValue = 33.44m;

        var target = new Position(id)
        {
            UnitPrice = unitPrice,
            Units = units,
            PriceDate = priceDate,
            Memo = memo,
            PositionType = positionType,
            SubAccountType = subAccountType,
            SecurityType = securityType,
            MarketValue = marketValue
        };

        Assert.AreEqual(id, target.SecurityId);
        Assert.AreEqual(unitPrice, target.UnitPrice);
        Assert.AreEqual(units, target.Units);
        Assert.AreEqual(priceDate, target.PriceDate);
        Assert.AreEqual(memo, target.Memo);
        Assert.AreEqual(positionType, target.PositionType);
        Assert.AreEqual(subAccountType, target.SubAccountType);
        Assert.AreEqual(securityType, target.SecurityType);
        Assert.AreEqual(marketValue, target.MarketValue);
    }

    [TestMethod]
    public void CollectionTest()
    {
        var target = new PositionList();
        Assert.IsEmpty(target.Items, "Collection must be intialize with zero entries.");
    }

}
