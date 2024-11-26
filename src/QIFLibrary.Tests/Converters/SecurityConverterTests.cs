using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace QIFLibrary.Tests.Converters;

[TestClass]
public class SecurityConverterTests
{
    [TestMethod]
    public void ConvertWithNoIdTest()
    {
        var root = new OFXProperty("SECLIST");
        var stock = new OFXProperty("STOCKINFO");
        var security = new OFXProperty("SECINFO");

        var securityId = new OFXProperty("SECID");
        //securityId.Children.Add(new OFXProperty("UNIQUEID", "ABC"));

        security.Children.Add(securityId);
        security.Children.Add(new OFXProperty("SECNAME", "ABC Company"));
        security.Children.Add(new OFXProperty("TICKER", "ABC"));
        security.Children.Add(new OFXProperty("UNITPRICE", "1.23"));
        security.Children.Add(new OFXProperty("DTASOF", "20241021104315"));

        stock.Children.Add(security);
        root.Children.Add(stock);
        root.Children.Add(stock);   // add twice to test list.

        var target = new SecurityConverter();
        var actual = target.Convert(root);

        Assert.IsNull(actual, "Cannot convert a stock with no identifier.");
    }

    [TestMethod]
    public void ConvertTest()
    {
        var root = new OFXProperty("SECLIST");
        var stock = new OFXProperty("STOCKINFO");
        var security = new OFXProperty("SECINFO");

        var securityId = new OFXProperty("SECID");
        securityId.Children.Add(new OFXProperty("UNIQUEID", "ABC"));
        securityId.Children.Add(new OFXProperty("UNIQUEIDTYPE", "TICKER"));

        security.Children.Add(securityId);
        security.Children.Add(new OFXProperty("SECNAME", "ABC Company"));
        security.Children.Add(new OFXProperty("TICKER", "ABC"));
        security.Children.Add(new OFXProperty("UNITPRICE", "1.23"));
        security.Children.Add(new OFXProperty("DTASOF", "20241021104315"));

        stock.Children.Add(security);
        root.Children.Add(stock);
        root.Children.Add(stock);   // add twice to test list.

        var target = new SecurityConverter();
        var actual = target.Convert(root);

        Assert.IsNotNull(actual);
        Assert.AreEqual("ABC", actual.Ticker);
        Assert.AreEqual("ABC Company", actual.Name);
        Assert.AreEqual(Security.SecurityIdTypes.TICKER, actual.SecurityIdType);
        Assert.AreEqual(new DateTime(2024, 10, 21, 10, 43, 15, DateTimeKind.Utc), actual.UnitPriceDate!.Value.ToUniversalTime());
        Assert.AreEqual(1.23m, actual.UnitPrice);
        Assert.AreEqual("ABC", actual.Id);

        var list = ((IPropertyConverter<SecurityList>)target).Convert(root);
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Items.Count);
    }

    [TestMethod]
    public void BadConvertTest()
    {
        Assert.IsNull(new SecurityConverter().Convert(new OFXProperty("bad name")));
        Assert.IsNull((new SecurityConverter() as IPropertyConverter<SecurityList>).Convert(new OFXProperty("Bad list")));

    }

    [TestMethod]
    public void ConverterTest()
    {
        var root = new OFXProperty("Bad Root");

        var target = new OFXPropertyConverter();
        var actual = target.GetSecurity(root);

        Assert.IsNull(actual);
        Assert.IsNull(target.GetSecurityList(root));
    }

    [TestMethod]
    public void ToPropertyTest()
    {
        var date = DateTime.Now;
        var name = "name";
        var tick = "ABC";
        var id = "id";
        var price = 123.45m;
        var sectype = Security.SecurityTypes.MUTUALFUND;
        var secIdType = Security.SecurityIdTypes.CUSIP;

        var data = new Security(id, tick, name, price, date)
        {
            SecurityType = sectype,
            SecurityIdType = secIdType
        };

        var actual = SecurityConverter.ToProperty(data);

        Assert.AreEqual("SECINFO", actual.Name);
        Assert.AreEqual("CUSIP", actual.Children["SECID"].Children["UNIQUEIDTYPE"].Value);
        Assert.AreEqual(tick, actual.Children["SECID"].Children["UNIQUEID"].Value);

        Assert.AreEqual(name, actual.Children["SECNAME"].Value);
        Assert.AreEqual(tick, actual.Children["TICKER"].Value);
        Assert.AreEqual(price, actual.Children["UNITPRICE"].AsDecimal());
        Assert.AreEqual(date.ToString(), actual.Children["DTASOF"].AsDate().ToString());
    }
}