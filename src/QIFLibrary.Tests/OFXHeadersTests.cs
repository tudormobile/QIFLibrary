using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXHeadersTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var target = new OFXHeaders();
        Assert.IsFalse(target.HasValue(""));
    }
    [TestMethod]
    public void ValueTest()
    {
        var key = "some key";
        var expected = "some value";
        var target = new OFXHeaders();

        Assert.IsFalse(target.HasValue(key));
        Assert.AreEqual(String.Empty, target[key]);
        target[key] = expected;
        Assert.IsTrue(target.HasValue(key));
        var actual = target[key];
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddTest()
    {
        var key = "some key";
        var expected = "some value";
        var target = new OFXHeaders();
        Assert.AreSame(target, target.Add(key, expected), "Failed to return reference to self.");
        Assert.AreSame(expected, target[key]);
    }

    [TestMethod]
    public void DefaultTest()
    {
        var expected = "102";
        var target = OFXHeaders.Default();
        Assert.AreEqual(expected, target["VERSION"]);
        Assert.AreNotSame(target, OFXHeaders.Default(), "Must return new instance for each default method call.");
    }

    [TestMethod]
    public void AsEnumerableTest()
    {
        var target = new OFXHeaders()
            .Add("one", "1")
            .Add("two", "2")
            .Add("three", "3");

        var actual = target.AsEnumerable().ToList();
        Assert.AreEqual(3, actual.Count);
    }
}
