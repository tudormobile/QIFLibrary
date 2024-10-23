using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Tests;

[TestClass]
public class OFXMessageSetTests
{
    [TestMethod]
    public void ConstructorTest1()
    {
        var version = 5;
        var direction = OFXMessageDirection.REQUEST;
        var type = OFXMessageSetTypes.SECLIST;

        var target = new OFXMessageSet(type, direction, version);

        Assert.AreEqual(version, target.Version);
        Assert.AreEqual(direction, target.Direction);
        Assert.AreEqual(type, target.MessageSetType);

        Assert.AreEqual(0, target.Messages.Count, "Messages must be initialized to empty collection.");
    }

    [TestMethod]
    public void ConstructorTest2()
    {
        var version = 1;
        var direction = OFXMessageDirection.RESPONSE;
        var type = OFXMessageSetTypes.UNKNOWN;

        var target = new OFXMessageSet(type, direction);

        Assert.AreEqual(version, target.Version, "Default value for 'Version' property must be 1.");
        Assert.AreEqual(direction, target.Direction);
        Assert.AreEqual(type, target.MessageSetType);
    }

}