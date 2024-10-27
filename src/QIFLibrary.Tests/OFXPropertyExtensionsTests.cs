﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyExtensionsTests
{
    [TestMethod]
    public void AsDateTest1()
    {
        var expected = new DateTime(1964, 3, 11, 2, 50, 0, DateTimeKind.Utc);

        var data = new string[]
        {
            // These are ALL THE SAME
            "196403110250",
            "19640311025000",
            "19640311025000.000",
            "19640311075000\t[-5:EST]",
            "19640311075000 [-5:EST]",
            "19640311015000 [ +1 : ??? ]",
        };

        foreach (var item in data)
        {
            var p = new OFXProperty("DT", item);
            var actual = p.AsDate().ToUniversalTime();
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void AsDateTest2()
    {
        var expected = new DateTime(1964, 3, 11, 2, 50, 0, DateTimeKind.Utc);
        var actual = new OFXProperty("", "").AsDate(expected);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AsDateTest3()
    {
        var expected = DateTime.UtcNow;
        var actual = new OFXProperty("", "").AsDate();
        Assert.AreEqual(expected.Ticks, actual.Ticks, 100);
    }

    [TestMethod]
    public void AsBooleanTest()
    {
        var trueData = new string[]
        {
            // These are ALL TRUE
            "True","T","t",
            "true","1","Yes", "yes",
            "Y", "y"
        };
        var falseData = new string[]
        {
            // These are ALL TRUE
            "False","F","f",
            "false","0","No", "no",
            "N", "n", "", "BAD VALUE"
        };

        foreach (var item in trueData)
        {
            var p = new OFXProperty("B", item);
            Assert.IsTrue(p.AsBoolean());
        }

        foreach (var item in falseData)
        {
            var p = new OFXProperty("B", item);
            Assert.IsFalse(p.AsBoolean());
        }

    }

    [TestMethod]
    public void AsDecimalTest1()
    {
        var expected = 1.23m;
        var p = new OFXProperty("d", expected.ToString());
        var actual = p.AsDecimal();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AsDecimalTest2()
    {
        var expected = 1.23m;
        var p = new OFXProperty("d", "bad value");
        var actual = p.AsDecimal(defaultValue: expected);
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(0m, p.AsDecimal());
    }

    [TestMethod]
    public void AsIntegerTest1()
    {
        var expected = 3456;
        var p = new OFXProperty("d", "3456");
        var actual = p.AsInteger();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AsIntegerTest2()
    {
        var expected = 3456;
        var p = new OFXProperty("d", "bad value");
        var actual = p.AsInteger(defaultValue: expected);
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(0, p.AsInteger());
    }
}
