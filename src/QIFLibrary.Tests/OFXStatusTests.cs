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
        public void ConstructorTest2()
        {
            var expected = "this is a message";
            var actual = new OFXStatus(expected);
            Assert.AreEqual(expected, actual.Message);
            Assert.AreEqual(OFXStatus.StatusSeverity.INFO, actual.Severity, "Must default to INFO status");
            Assert.AreEqual(0, actual.Code, "Must default to code zero.");
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

        [TestMethod]
        public void ToStringTest()
        {
            var target = new OFXStatus()
            {
                Severity = OFXStatus.StatusSeverity.UNKNOWN,
                Code = 123,
                Message = "",
            };
            var actual = target.ToString();

            Assert.IsTrue(actual.Contains("123"));
            Assert.IsTrue(actual.Contains("UNKNOWN"));
            Assert.IsFalse(actual.Contains("MESSAGE"));

            target.Message = "m";
            Assert.IsTrue(target.ToString().Contains("MESSAGE"));
        }

        [TestMethod]
        public void ToStringsTest()
        {
            var target = new OFXStatus()
            {
                Severity = OFXStatus.StatusSeverity.UNKNOWN,
                Code = 123,
                Message = "this is a test",
            };
            var actual = target.ToStrings();
            Assert.AreEqual(5, actual.Count());
        }

    }
}
