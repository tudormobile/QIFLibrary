using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXSecurityListTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var ticker = "MSFT";
        var name = "Microsoft Corp.";
        var price = 408.46m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);

        var securityList = new SecurityList();

        securityList.Items.Add(new Security(ticker, ticker, name, price, priceDate) { SecurityType = Security.SecurityTypes.STOCK });

        var target = new OFXSecurityList(securityList);

        Assert.AreEqual("SECLIST", target.Name);
        Assert.HasCount(1, target.Properties);
    }
}