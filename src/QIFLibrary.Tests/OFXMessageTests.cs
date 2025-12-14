using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXMessageTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var target = new OFXMessage();
        Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, target.Status.Severity, "Default severity must be unknown.");
        Assert.AreEqual(string.Empty, target.Name, "Default value must be empty string.");
        Assert.AreEqual(string.Empty, target.Id, "Default value must be empty string.");
    }

    [TestMethod]
    public void NameTest()
    {
        var expected = "name";
        var target = new OFXMessage() { Name = expected };
        var actual = target.Name;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void IdTest()
    {
        var expected = "123ABC";
        var target = new OFXMessage() { Id = expected };
        var actual = target.Id;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void StatusTest()
    {
        var expected = new OFXStatus();
        var target = new OFXMessage() { Status = expected };
        var actual = target.Status;
        Assert.AreEqual(expected.Message, actual.Message);
        Assert.AreEqual(expected.Severity, actual.Severity);
        Assert.AreEqual(expected.Code, actual.Code);
    }

    [TestMethod]
    public void ToStringTest()
    {
        var expected = "123";
        var id = "ABC";
        var status = new OFXStatus() { Code = 123 };
        var target = new OFXMessage() { Name = "NAME", Status = status };
        target.Properties.Add(new OFXProperty("Child", "Property"));
        var actual = target.ToString();
        Assert.DoesNotContain(expected, actual, "Should NOT contain status when code is unknown.");
        Assert.DoesNotContain(id, actual, "Must NOT contain Id when it is not set.");

        target.Id = id;
        status.Severity = OFXStatus.StatusSeverity.WARN;
        actual = target.ToString();
        Assert.Contains(expected, actual, "MUST contain status when code is known.");
        Assert.Contains(id, actual, "MUST contain Id.");

        var strings = target.ToStrings();
        Assert.AreEqual(8, strings.Count(), "Failed to produce correct number of strings.");
    }

}
