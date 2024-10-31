using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyCollectionTests
{
    [TestMethod]
    public void ItemTest1()
    {
        var name = "some name";
        var target = new OFXPropertyCollection();
        var actual = target[name];
        Assert.AreEqual(name, actual.Name);
        Assert.AreEqual(String.Empty, actual.Value);
    }

    [TestMethod]
    public void ItemTest2()
    {
        var name = "some name";
        var value = "some value";
        var prop = new OFXProperty(name, value);
        var target = new OFXPropertyCollection();
        target.Add(prop);
        var actual = target[name];
        Assert.AreEqual(name, actual.Name);
        Assert.AreEqual(value, actual.Value);
    }

}
