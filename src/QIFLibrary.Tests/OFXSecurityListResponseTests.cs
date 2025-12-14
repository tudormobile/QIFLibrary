using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXSecurityListResponseTests
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

        var target = new OFXSecurityListResponse(securityList);

        Assert.AreEqual(1, target.Version, "Must default to version 1.");
        Assert.AreEqual(OFXMessageSetTypes.SECLIST, target.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.RESPONSE, target.Direction);
        Assert.HasCount(1, target.Messages);
    }

    [TestMethod]
    public void ConstructorWithCookieTest()
    {
        var cookie = "cookie value";

        var ticker = "MSFT";
        var name = "Microsoft Corp.";
        var price = 408.46m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);

        var securityList = new SecurityList();

        securityList.Items.Add(new Security(ticker, ticker, name, price, priceDate) { SecurityType = Security.SecurityTypes.STOCK });

        var target = new OFXSecurityListResponse(securityList, cookie: cookie);

        Assert.AreEqual(1, target.Version, "Must default to version 1.");
        Assert.AreEqual(OFXMessageSetTypes.SECLIST, target.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.RESPONSE, target.Direction);
        Assert.HasCount(2, target.Messages);

        Assert.AreEqual(cookie, target.Messages[0].Properties.First(p => p.Name == "CLTCOOKIE").Value);
        Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, target.Messages[0].Status.Severity, "Must use unknown status if not specified.");
        Assert.HasCount(1, target.Messages[0].Properties, "Should ONLY have the cookie property.");
    }

    [TestMethod]
    public void ConstructorWithUserIdTest()
    {
        var userId = "1234";

        var ticker = "MSFT";
        var name = "Microsoft Corp.";
        var price = 408.46m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);

        var securityList = new SecurityList();

        securityList.Items.Add(new Security(ticker, ticker, name, price, priceDate) { SecurityType = Security.SecurityTypes.STOCK });

        var target = new OFXSecurityListResponse(securityList, userId: userId);

        Assert.AreEqual(1, target.Version, "Must default to version 1.");
        Assert.AreEqual(OFXMessageSetTypes.SECLIST, target.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.RESPONSE, target.Direction);
        Assert.HasCount(2, target.Messages);

        Assert.AreEqual(userId, target.Messages[0].Id);
        Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, target.Messages[0].Status.Severity, "Must use unknown status if not specified.");
        Assert.IsEmpty(target.Messages[0].Properties, "Should not have any properties set.");
    }

    [TestMethod]
    public void ConstructorWithStatusTest()
    {
        var status = new OFXStatus()
        {
            Code = 1,
            Severity = OFXStatus.StatusSeverity.ERROR,
            Message = "This is a test"
        };

        var ticker = "MSFT";
        var name = "Microsoft Corp.";
        var price = 408.46m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);

        var securityList = new SecurityList();

        securityList.Items.Add(new Security(ticker, ticker, name, price, priceDate) { SecurityType = Security.SecurityTypes.STOCK });

        var target = new OFXSecurityListResponse(securityList, status: status);

        Assert.AreEqual(1, target.Version, "Must default to version 1.");
        Assert.AreEqual(OFXMessageSetTypes.SECLIST, target.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.RESPONSE, target.Direction);
        Assert.HasCount(2, target.Messages);

        Assert.AreEqual(status.Severity, target.Messages[0].Status.Severity);
        Assert.AreEqual(status.Message, target.Messages[0].Status.Message);
        Assert.AreEqual(status.Code, target.Messages[0].Status.Code);
        Assert.IsEmpty(target.Messages[0].Properties, "Should not have any properties set.");
    }

}