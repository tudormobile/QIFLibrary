using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests
{
    [TestClass]
    public class OFXCurrencyTypeTests
    {
        [TestMethod]
        public void DescriptionTest()
        {
            var data = new (int, string, OFXCurrencyType)[]
                {
                    (36, "Australian dollar", OFXCurrencyType.AUD),
                    (124, "Canadian dollar", OFXCurrencyType.CAD),
                    (756, "Swiss franc", OFXCurrencyType.CHF),
                    (978, "Euro", OFXCurrencyType.EUR),
                    (840, "US dollar", OFXCurrencyType.USD),
                    (484,"Mexican peso", OFXCurrencyType.MXN),
                    (826, "British pound sterling", OFXCurrencyType.GBP),
                    (392, "Japanese yen" , OFXCurrencyType.JPY),
                    (0, "UNKNOWN" , OFXCurrencyType.UNKNOWN),
                };

            foreach (var item in data)
            {
                var cur = (OFXCurrencyType)item.Item1;
                Assert.AreEqual(item.Item2, cur.Description());
            }

        }
    }
}
