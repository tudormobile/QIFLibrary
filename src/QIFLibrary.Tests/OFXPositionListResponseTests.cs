using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPositionListResponseTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var ticker = "MSFT";
        var price = 431.95m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);
        var units = 0.040m;

        var positionList = new PositionList();

        positionList.Items.Add(new Position(ticker)
        {
            SecurityType = Security.SecurityTypes.STOCK,
            SubAccountType = Position.PositionAccountTypes.CASH,
            MarketValue = price * units,
            PriceDate = priceDate,
            PositionType = Position.PositionTypes.LONG,
            UnitPrice = price,
            Units = units,
        });
        var account = new Account() { AccountId = "123", AccountType = Account.AccountTypes.INVESTMENT, InstitutionId = "456" };
        var date = DateTime.Now;
        var cookie = "cookie";
        var status = new OFXStatus() { Code = 1, Message = "message", Severity = OFXStatus.StatusSeverity.WARN };
        var currency = OFXCurrencyType.JPY;
        var id = "id";
        var target = new OFXPositionListResponse(positionList, account, date, id, cookie, status, currency);


        Assert.AreEqual(date, target.DateAsOf);
        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(cookie, target.Cookie);
        Assert.AreEqual(currency, target.Currency);
        Assert.AreEqual(status.Message, target.Status.Message);
        Assert.AreEqual("INVSTMTTRNRS", target.Name);
    }

    [TestMethod]
    public void ConstructorOptionalParametersMissingTest()
    {
        var ticker = "MSFT";
        var price = 431.95m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);
        var units = 0.040m;

        var positionList = new PositionList();
        positionList.Items.Add(new Position(ticker)
        {
            SecurityType = Security.SecurityTypes.STOCK,
            SubAccountType = Position.PositionAccountTypes.CASH,
            MarketValue = price * units,
            PriceDate = priceDate,
            PositionType = Position.PositionTypes.LONG,
            UnitPrice = price,
            Units = units,
        });

        var account = new Account()
        {
            AccountId = "123",
            AccountType = Account.AccountTypes.MONEYMRKT,
            InstitutionId = "456"
        };

        var beforeTime = DateTime.Now;  // ← Capture BEFORE construction
        var target = new OFXPositionListResponse(positionList, account);
        var afterTime = DateTime.Now;   // ← Capture AFTER construction

        // Assert that the DateAsOf is within the time window
        Assert.IsTrue(target.DateAsOf >= beforeTime);
        Assert.IsTrue(target.DateAsOf <= afterTime);

        Assert.AreEqual(String.Empty, target.Id);
        Assert.AreEqual(String.Empty, target.Cookie);
        Assert.AreEqual(OFXCurrencyType.USD, target.Currency);
        Assert.AreEqual(String.Empty, target.Status.Message);
        Assert.AreEqual(0, target.Status.Code);
        Assert.AreEqual(OFXStatus.StatusSeverity.INFO, target.Status.Severity);
        Assert.AreEqual("INVSTMTTRNRS", target.Name);

        Assert.DoesNotContain("CLTCOOKIE", target.ToString(), "Must NOT have a cookie in this case.");
    }

    [TestMethod]
    public void PropertiesTest()
    {
        var ticker = "MSFT";
        var price = 431.95m;
        var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);
        var units = 0.040m;

        var positionList = new PositionList();

        positionList.Items.Add(new Position(ticker)
        {
            SecurityType = Security.SecurityTypes.STOCK,
            SubAccountType = Position.PositionAccountTypes.CASH,
            MarketValue = price * units,
            PriceDate = priceDate,
            PositionType = Position.PositionTypes.LONG,
            UnitPrice = price,
            Units = units,
        });
        var account = new Account() { AccountId = "123", AccountType = Account.AccountTypes.INVESTMENT, InstitutionId = "456" };
        var date = DateTime.Now;
        var cookie = "cookie";
        var status = new OFXStatus() { Code = 1, Message = "message", Severity = OFXStatus.StatusSeverity.WARN };
        var currency = OFXCurrencyType.JPY;
        var id = "id";
        var target = new OFXPositionListResponse(positionList, account, date, id, cookie, status, currency);

        var actual = target.ToString();

        Assert.IsNotNull(actual);
        Assert.Contains("CLTCOOKIE", actual);
        Assert.Contains("TRNUID", actual);
        Assert.Contains(id, actual);
        Assert.Contains(cookie, actual);

        Assert.Contains("INVACCTFROM", actual);
        Assert.Contains("123", actual);
        Assert.Contains("456", actual);

    }

}