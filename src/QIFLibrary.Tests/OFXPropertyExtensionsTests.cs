using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;
using static Tudormobile.QIFLibrary.Entities.Position;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyExtensionsTests
{
    [TestMethod]
    public void HasChildrenTest()
    {
        var name = "name";
        var target = new OFXProperty(name);
        Assert.IsFalse(target.HasChildren());
        target.Children.Add(new OFXProperty(name));
        Assert.IsTrue(target.HasChildren());
    }

    [TestMethod]
    public void ListAddLanguageTest()
    {
        var target = new List<OFXProperty>().Add(new OFXLanguage());
        Assert.AreEqual("LANGUAGE", target.First().Name);
    }

    [TestMethod]
    public void ListAddInstitutionTest()
    {
        var target = new List<OFXProperty>().Add(new Institution());
        Assert.AreEqual("FI", target.First().Name);
    }

    [TestMethod]
    public void ListAddDateTest()
    {
        var expected = "suffix";
        var target = new List<OFXProperty>().Add(DateTime.Now, expected);
        var actual = target.First().Name;

        Assert.IsTrue(actual.StartsWith("DT"));
        Assert.IsTrue(actual.EndsWith(expected));
    }

    [TestMethod]
    public void AsPositionAccountTypeTest()
    {
        var expected = OFXPositionAccountTypes.MARGIN;
        var target = new OFXProperty("name", expected.ToString());

        var actual = target.AsPositionAccountType();
        Assert.AreEqual(expected, actual);

        target.Value = "bad value";
        Assert.AreEqual(OFXPositionAccountTypes.OTHER, target.AsPositionAccountType());
    }

    [TestMethod]
    public void AsPositionTypeTest()
    {
        var expected = OFXPositionTypes.LONG;
        var target = new OFXProperty("name", expected.ToString());

        var actual = target.AsPositionType();
        Assert.AreEqual(expected, actual);

        target.Value = "bad value";
        Assert.AreEqual(OFXPositionTypes.UNKNOWN, target.AsPositionType());
    }

    [TestMethod]
    public void AsAccountTypeTest()
    {
        var expected = OFXAccountType.CREDITLINE;
        var target = new OFXProperty("name", expected.ToString());

        var actual = target.AsAccountType();
        Assert.AreEqual(expected, actual);

        target.Value = "bad value";
        Assert.AreEqual(OFXAccountType.UNKNOWN, target.AsAccountType());
    }

    [TestMethod]
    public void AsLanguageTest()
    {
        var target = new OFXProperty("name", "ENG");
        Assert.AreEqual(OFXLanguage.ENG, target.AsLanguage());

        target.Value = "bad value";
        Assert.AreEqual(OFXLanguage.UNKNOWN, target.AsLanguage());
    }

    [TestMethod]
    public void IsEmptyTest()
    {
        var target = new OFXProperty("name");
        Assert.IsTrue(target.IsEmpty());
        Assert.IsFalse(target.HasValue());
    }

    [TestMethod]
    public void HasValueTest()
    {
        var target = new OFXProperty("name", "value");
        Assert.IsFalse(target.IsEmpty());
        Assert.IsTrue(target.HasValue());
    }

    [TestMethod]
    public void AsTransactionTypeTest()
    {
        var name = "Some Name";
        var value = "Fee";
        var expected = OFXTransactionType.FEE;

        var target = new OFXProperty(name, value);
        Assert.AreEqual(expected, target.AsTransactionType());

        // check the other type
        var actual = new OFXProperty("Bad", "Value").AsTransactionType();
        Assert.AreEqual(OFXTransactionType.OTHER, actual);
    }

    [TestMethod]
    public void AsCurrencyTest()
    {
        var expected = OFXCurrencyType.USD;
        var target = new OFXProperty("name", "value");
        Assert.AreEqual(expected, target.AsCurrency(expected), "Failed too utilize default value.");
        Assert.AreEqual(expected, target.AsCurrency(), "Failed too utilize default value USD when unrecognized and not explicit default provided.");

        var actual = new OFXProperty("CURDEF", "CaD").AsCurrency(); // case should not matter?
        Assert.AreEqual(OFXCurrencyType.CAD, actual);
    }

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
