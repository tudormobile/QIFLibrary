using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXSecurityTests
{
    [TestMethod]
    public void OFXSecurityTest()
    {
        var ticker = "MSFT";
        var name = "Microsoft Corp.";
        var price = 408.46m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);

        var target = new OFXSecurity(new Security(ticker, ticker, name, price, priceDate) { SecurityType = Security.SecurityTypes.STOCK });

        Assert.AreEqual("STOCKINFO", target.Name);
        Assert.HasCount(1, target.Children);
        Assert.AreEqual("SECINFO", target.Children[0].Name);
        Assert.HasCount(5, target.Children[0].Children);
    }
}