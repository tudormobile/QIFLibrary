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
        Assert.IsEmpty(target.Children, "Must initialize children to an empty list.");

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

    [TestMethod]
    public void SimpleToStringTest()
    {
        var name = "some name";
        var value = "some value";

        var target = new OFXProperty(name, value);
        var actual = target.ToString();

        Assert.Contains(name.ToUpperInvariant(), actual);
        Assert.Contains(value, actual);
    }

    [TestMethod]
    public void ComplexToStringTest()
    {
        var name = "some name";
        var value = "some value";

        var target = new OFXProperty(name, value);
        target.Children.Add(new OFXProperty("child", "data"));
        var actual = target.ToString();

        Assert.Contains(name.ToUpperInvariant(), actual);
        Assert.Contains("CHILD", actual);
        Assert.Contains("data", actual);
        Assert.DoesNotContain(value, actual);

        Assert.AreEqual(3, target.ToStrings().Count());
    }

}
