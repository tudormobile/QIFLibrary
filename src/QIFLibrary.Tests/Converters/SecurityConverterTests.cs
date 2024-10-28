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

namespace QIFLibrary.Tests
{
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
            Assert.AreEqual(new DateTime(2024, 10, 21, 6, 43, 15), actual.UnitPriceDate);
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
    }
}