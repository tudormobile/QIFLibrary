using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var name = "some name";
        var value = "some value";

        var target = new OFXProperty(name, value);

        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(value, target.Value);
        Assert.AreEqual(0, target.Children.Count, "Must initialize children to an empty list.");

    }

    [TestMethod]
    public void NameTest()
    {
        var name = "some name";
        var target = new OFXProperty("name")
        {
            Name = name
        };
        var actual = target.Name;
        Assert.AreEqual(name, actual);
    }

    [TestMethod]
    public void ValueTest()
    {
        var name = "some name";
        var value = "some value";

        var target = new OFXProperty(name)
        {
            Value = value
        };
        var actual = target.Value;
        Assert.AreEqual(value, actual);
    }
}
