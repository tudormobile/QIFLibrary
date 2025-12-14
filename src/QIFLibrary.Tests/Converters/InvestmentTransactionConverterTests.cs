using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace QIFLibrary.Tests.Converters;

[TestClass]
public class InvestmentTransactionConverterTests
{
    [TestMethod]
    public void ConvertTest()
    {
        var root = new OFXProperty("INVTRANLIST");
        root.Children.Add(new OFXProperty("DTSTART", "20241021104315"));
        root.Children.Add(new OFXProperty("DTEND", "20241021104316"));

        var wrapper = new OFXProperty("SELLMF");
        var invsell = new OFXProperty("INVSELL");
        var invtran = new OFXProperty("INVTRAN");
        var secid = new OFXProperty("SECID");

        secid.Children.Add(new OFXProperty("UNIQUEID", "MSFT"));

        invtran.Children.Add(new OFXProperty("FITID", "FID"));
        invtran.Children.Add(new OFXProperty("DTTRADE", "20241112"));

        invsell.Children.Add(secid);
        invsell.Children.Add(invtran);
        invsell.Children.Add(new OFXProperty("UNITS", "123"));
        invsell.Children.Add(new OFXProperty("UNITPRICE", "1.234"));
        invsell.Children.Add(new OFXProperty("TOTAL", "543.21"));

        wrapper.Children.Add(invsell);
        root.Children.Add(wrapper);

        var target = new InvestmentTransactionConverter();
        var actual = target.Convert(wrapper);

        Assert.IsNotNull(actual);
        Assert.AreEqual("FID", actual.Id);
        Assert.AreEqual("MSFT", actual.SecurityId);
        Assert.AreEqual(1.234m, actual.UnitPrice);
        Assert.AreEqual(123m, actual.Units);
        Assert.AreEqual(543.21m, actual.Total);
        Assert.AreEqual(new DateTime(2024, 11, 12, 0, 0, 0, DateTimeKind.Local), actual.DatePosted);
        Assert.AreEqual(InvestmentTransactionType.SELLMF, actual.TransactionType);

        var list = (target as IPropertyConverter<InvestmentTransactionList>).Convert(root);
        Assert.IsNotNull(list);
        Assert.HasCount(1, list.Items);
        Assert.AreEqual(new DateTime(2024, 10, 21, 10, 43, 15, DateTimeKind.Utc).ToLocalTime(), list.Start);
        Assert.AreEqual(new DateTime(2024, 10, 21, 10, 43, 16, DateTimeKind.Utc).ToLocalTime(), list.End);
    }

    [TestMethod]
    public void ConvertyWithMissingSecurityIdTest()
    {
        var wrapper = new OFXProperty("SELLMF");
        var invsell = new OFXProperty("INVSELL");
        var invtran = new OFXProperty("INVTRAN");
        var secid = new OFXProperty("SECID");

        //secid.Children.Add(new OFXProperty("UNIQUEID", "MSFT"));

        invtran.Children.Add(new OFXProperty("FITID", "FID"));
        invtran.Children.Add(new OFXProperty("DTTRADE", "20241112"));

        invsell.Children.Add(secid);
        invsell.Children.Add(invtran);
        invsell.Children.Add(new OFXProperty("UNITS", "123"));
        invsell.Children.Add(new OFXProperty("UNITPRICE", "1.234"));
        invsell.Children.Add(new OFXProperty("TOTAL", "543.21"));

        wrapper.Children.Add(invsell);

        var target = new InvestmentTransactionConverter();
        var actual = target.Convert(wrapper);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ConvertyWithMissingSTransactionTypeTest()
    {
        var wrapper = new OFXProperty("SEXXXXXLLMF");
        var invsell = new OFXProperty("INVSELL");
        var invtran = new OFXProperty("INVTRAN");
        var secid = new OFXProperty("SECID");

        secid.Children.Add(new OFXProperty("UNIQUEID", "MSFT"));

        invtran.Children.Add(new OFXProperty("FITID", "FID"));
        invtran.Children.Add(new OFXProperty("DTTRADE", "20241112"));

        invsell.Children.Add(secid);
        invsell.Children.Add(invtran);
        invsell.Children.Add(new OFXProperty("UNITS", "123"));
        invsell.Children.Add(new OFXProperty("UNITPRICE", "1.234"));
        invsell.Children.Add(new OFXProperty("TOTAL", "543.21"));

        wrapper.Children.Add(invsell);

        var target = new InvestmentTransactionConverter();
        var actual = target.Convert(wrapper);

        Assert.IsNull(actual);

    }

    [TestMethod]
    public void BadConvertTest()
    {
        Assert.IsNull(new InvestmentTransactionConverter().Convert(new OFXProperty("bad name")));
        Assert.IsNull((new InvestmentTransactionConverter() as IPropertyConverter<InvestmentTransactionList>).Convert(new OFXProperty("Bad list")));
    }

    [TestMethod]
    public void ConverterTest()
    {
        var root = new OFXProperty("Bad Root");

        var target = new OFXPropertyConverter();
        var actual = target.GetInvestmentTransaction(root);

        Assert.IsNull(actual);
        Assert.IsNull(target.GetInvestmentTransactionList(root));
    }

}