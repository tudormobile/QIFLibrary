using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests
{
    [TestClass]
    public class OFXStatusTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var target = new OFXStatus();

            Assert.AreEqual(0, target.Code, "Default code must be zero");
            Assert.AreEqual(String.Empty, target.Message, "Default message must be empty string");
            Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, target.Severity, "Default severity must be UNKNOWN");
        }

        [TestMethod]
        public void CodeTest()
        {
            var expected = 123;
            var target = new OFXStatus() { Code = expected };
            var actual = target.Code;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MessageTest()
        {
            var expected = "some message";
            var target = new OFXStatus() { Message = expected };
            var actual = target.Message;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SeverityTest()
        {
            var expected = OFXStatus.StatusSeverity.ERROR;
            var target = new OFXStatus() { Severity = expected };
            var actual = target.Severity;
            Assert.AreEqual(expected, actual);
        }

    }
}
