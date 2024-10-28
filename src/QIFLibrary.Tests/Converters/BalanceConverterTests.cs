using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;

namespace QIFLibrary.Tests.Converters
{
    [TestClass]
    public class BalanceConverterTests
    {
        [TestMethod]
        public void ConvertTest1()
        {
            var root = new OFXProperty("AVAILBAL");
            root.Children.Add(new OFXProperty("BALAMT", "-123.45"));
            root.Children.Add(new OFXProperty("DTASOF", "20241021104315[-5:EST]"));
            var target = new BalanceConverter();
            var actual = target.Convert(root);

            Assert.AreEqual(-123.45m, actual!.Value);
            Assert.AreEqual(new DateTime(2024, 10, 21, 1, 43, 15), actual.Value.Date);
        }

        [TestMethod]
        public void ConvertTest2()
        {
            var root1 = new OFXProperty("AVAILBAL");
            root1.Children.Add(new OFXProperty("BALAMT", "-123.45"));
            root1.Children.Add(new OFXProperty("DTASOF", "20241021104315[-5:EST]"));

            var root2 = new OFXProperty("LEDGERBAL");

            var root = new OFXProperty("");
            root.Children.Add(root2);
            root.Children.Add(root1);

            var expected = 0m;
            var target = new BalanceConverter();
            var actual = target.Convert(root);

            Assert.AreEqual(expected, actual!.Value);
            Assert.IsNull(actual.Value.Date);

        }

        [TestMethod]
        public void ConvertTest3()
        {
            Assert.IsNull(new BalanceConverter().Convert(new OFXProperty("not valid")));
        }

        [TestMethod]
        public void ConverterTest()
        {
            var root = new OFXProperty("LEDGERBAL");
            root.Children.Add(new OFXProperty("BALAMT", "-123.45"));
            root.Children.Add(new OFXProperty("DTASOF", "20241021104315[-5:EST]"));

            var target = new OFXPropertyConverter();
            var actual = target.GetBalance(root);

            Assert.AreEqual(-123.45m, actual!.Value);
            Assert.AreEqual(new DateTime(2024, 10, 21, 1, 43, 15), actual.Value.Date);

        }

    }
}
