﻿using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

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

    [TestMethod]
    public void ToStringTest1()
    {
        var direction = OFXMessageDirection.RESPONSE;
        var type = OFXMessageSetTypes.UNKNOWN;

        var target = new OFXMessageSet(type, direction);
        target.Messages.Add(new OFXMessage() { Name = "child" });

        var actual = target.ToString();

        Assert.IsTrue(actual.Contains("UNKNOWNMSGSRSV1"));
        Assert.IsTrue(actual.Contains("CHILD"));

        Assert.AreEqual(4, target.ToStrings().Count());
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var direction = OFXMessageDirection.REQUEST;
        var type = OFXMessageSetTypes.UNKNOWN;

        var target = new OFXMessageSet(type, direction);
        target.Messages.Add(new OFXMessage() { Name = "child" });

        var actual = target.ToString();

        Assert.IsTrue(actual.Contains("UNKNOWNMSGSRQV1"));
        Assert.IsTrue(actual.Contains("CHILD"));

        Assert.AreEqual(4, target.ToStrings().Count());
    }

}