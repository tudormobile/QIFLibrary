using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace QIFLibrary.Tests.Converters;

[TestClass]
public class PositionConverterTests
{
    [TestMethod]
    public void ConvertTest()
    {
        var root = new OFXProperty("INVPOSLIST");

        // One of each
        var wrappers = "POSMF|POSSTOCK|POSDEBT|POSOPT|POSOTHER|BADNAME";

        foreach (var wrapperName in wrappers.Split('|'))
        {
            var expectedType = wrapperName switch
            {
                "POSMF" => Security.SecurityTypes.MUTUALFUND,
                "POSSTOCK" => Security.SecurityTypes.STOCK,
                "POSDEBT" => Security.SecurityTypes.DEBT,
                "POSOPT" => Security.SecurityTypes.OPTION,
                "POSOTHER" => Security.SecurityTypes.OTHER,
                _ => Security.SecurityTypes.UNKNOWN,
            };

            var wrapper = new OFXProperty(wrapperName);

            var position = new OFXProperty("INVPOS");
            var security = new OFXProperty("SECID");
            security.Children.Add(new OFXProperty("UNIQUEID", "AAPL"));

            position.Children.Add(security);
            position.Children.Add(new OFXProperty("HELDINACCT", "CASH"));
            position.Children.Add(new OFXProperty("POSTYPE", "LONG"));
            position.Children.Add(new OFXProperty("UNITS", "456"));
            position.Children.Add(new OFXProperty("UNITPRICE", "1.23"));
            position.Children.Add(new OFXProperty("MKTVAL", "112233.44"));
            position.Children.Add(new OFXProperty("MEMO", "Some Memo"));
            position.Children.Add(new OFXProperty("DTPRICEASOF", "20241004000000"));

            wrapper.Children.Add(position);

            root.Children.Clear();
            root.Children.Add(wrapper);

            var target = new PositionConverter();
            var actual = target.Convert(root);

            if (expectedType == Security.SecurityTypes.UNKNOWN)
            {
                Assert.IsNull(actual);
                break;
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedType, actual.SecurityType);
            Assert.AreEqual("AAPL", actual.SecurityId);
            Assert.AreEqual(Position.PositionAccountTypes.CASH, actual.SubAccountType);
            Assert.AreEqual(456m, actual.Units);
            Assert.AreEqual(1.23m, actual.UnitPrice);
            Assert.AreEqual(112233.44m, actual.MarketValue);
            Assert.AreEqual("Some Memo", actual.Memo);
            Assert.AreEqual(new DateTime(2024, 10, 4, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(), actual.PriceDate);
            Assert.AreEqual(Position.PositionTypes.LONG, actual.PositionType);

            var list = (target as IPropertyConverter<PositionList>).Convert(root);
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Items.Count);
        }
    }


    [TestMethod]
    public void ConvertWithNoSecurityIdTest()
    {
        var root = new OFXProperty("INVPOSLIST");

        var wrapper = new OFXProperty("POSSTOCK");

        var position = new OFXProperty("INVPOS");
        var security = new OFXProperty("SECID");
        //security.Children.Add(new OFXProperty("UNIQUEID", "AAPL"));

        position.Children.Add(security);
        position.Children.Add(new OFXProperty("HELDINACCT", "CASH"));
        position.Children.Add(new OFXProperty("POSTYPE", "LONG"));
        position.Children.Add(new OFXProperty("UNITS", "456"));
        position.Children.Add(new OFXProperty("UNITPRICE", "1.23"));
        position.Children.Add(new OFXProperty("MKTVAL", "112233.44"));
        position.Children.Add(new OFXProperty("MEMO", "Some Memo"));
        position.Children.Add(new OFXProperty("DTPRICEASOF", "20241004000000"));

        wrapper.Children.Add(position);
        root.Children.Add(wrapper);

        var target = new PositionConverter();
        var actual = target.Convert(root);

        Assert.IsNull(actual, "Cannot create a position without a security Id.");
    }

    [TestMethod]
    public void BadConvertTest()
    {
        Assert.IsNull(new PositionConverter().Convert(new OFXProperty("bad name")));
        Assert.IsNull((new PositionConverter() as IPropertyConverter<PositionList>).Convert(new OFXProperty("Bad list")));
    }

    [TestMethod]
    public void ConverterTest()
    {
        var root = new OFXProperty("Bad Root");

        var target = new OFXPropertyConverter();
        var actual = target.GetPosition(root);

        Assert.IsNull(actual);
        Assert.IsNull(target.GetPositionList(root));
    }

}