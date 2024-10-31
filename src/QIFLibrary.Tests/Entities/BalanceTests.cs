using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests.Entities
{
    [TestClass]
    public class BalanceTests
    {
        [TestMethod]
        public void BalanceTest()
        {
            var expected = 123.45m;
            var target = new Balance(expected);
            var actual = target.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected, target);
            Assert.IsNull(target.Date);

        }

        [TestMethod]
        public void DateTest()
        {
            var expected = DateTime.Now;
            var target = new Balance(0m, expected);

            Assert.AreEqual(expected, target.Date);

        }

    }
}
