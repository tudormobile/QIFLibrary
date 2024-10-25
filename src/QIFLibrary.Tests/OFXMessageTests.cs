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

}
