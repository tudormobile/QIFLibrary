using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyExtensionsTests
{
    [TestMethod]
    public void AddDateTest()
    {
        var date = new DateTime(1964, 3, 11, 1, 2, 3, DateTimeKind.Utc);
        var target = new List<OFXProperty>();

        target.Add(date, "ASOF");

        Assert.AreEqual("DTASOF", target[0].Name);
        Assert.AreEqual("19640311010203", target[0].Value);
    }

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
        var actual = new OFXProperty("", "").AsDate();
        var expected = DateTime.UtcNow;
        Assert.AreEqual(expected.Ticks, actual.Ticks, 1000);
    }

    [TestMethod]
    public void AsDateTest4()
    {
        var data = "20250917000000[-5:EST]";
        var expected = new DateTime(2025, 9, 17, 0, 0, 0, DateTimeKind.Unspecified);
        var actual = new OFXProperty("DT", data).AsDate();
        Assert.AreEqual(expected, actual, "Missing time data should ignore time.");
    }

    [TestMethod]
    public void AsDateTest5()
    {
        var data = "20250131000000[-5:EST]";
        var expected = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Unspecified);
        var actual = new OFXProperty("DT", data).AsDate();
        Assert.AreEqual(expected, actual, "Missing time data should ignore time.");
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

    [TestMethod]
    public void AddAccountTest1()
    {
        var expected = new Account()
        {
            AccountId = "123",
            InstitutionId = "356",
            AccountType = Account.AccountTypes.SAVINGS
        };
        var target = new List<OFXProperty>();
        var actual = target.Add(expected, OFXMessageDirection.REQUEST);

        Assert.AreSame(target, actual, "Failed to return reference to self.");

        Assert.AreEqual("BANKACCTTO", actual[0].Name);
        Assert.AreEqual("123", actual[0].Children["ACCTID"].Value);
        Assert.AreEqual("356", actual[0].Children["BROKERID"].Value);
    }

    [TestMethod]
    public void AddAccountTest2()
    {
        var expected = new Account()
        {
            AccountId = "123",
            InstitutionId = "356",
            AccountType = Account.AccountTypes.CREDITLINE
        };
        var target = new List<OFXProperty>();
        var actual = target.Add(expected, OFXMessageDirection.RESPONSE);

        Assert.AreSame(target, actual, "Failed to return reference to self.");

        Assert.AreEqual("CCACCTFROM", actual[0].Name);
        Assert.AreEqual("123", actual[0].Children["ACCTID"].Value);
        Assert.AreEqual("356", actual[0].Children["BROKERID"].Value);
    }

    [TestMethod]
    public void AddAccountTest3()
    {
        var expected = new Account()
        {
            AccountId = "123",
            InstitutionId = "356",
            AccountType = Account.AccountTypes.INVESTMENT
        };
        var target = new List<OFXProperty>();
        var actual = target.Add(expected, OFXMessageDirection.REQUEST);

        Assert.AreSame(target, actual, "Failed to return reference to self.");

        Assert.AreEqual("INVACCTTO", actual[0].Name);
        Assert.AreEqual("123", actual[0].Children["ACCTID"].Value);
        Assert.AreEqual("356", actual[0].Children["BROKERID"].Value);
    }


    [TestMethod]
    public void AddCurrencyTest()
    {
        var target = new List<OFXProperty>();
        var actual = target.Add(OFXCurrencyType.CAD);

        Assert.AreSame(target, actual, "Failed to return reference to self.");
        Assert.AreEqual("CURDEF", actual[0].Name);
        Assert.AreEqual("CAD", actual[0].Value);
    }

    [TestMethod]
    public void AddDecimalTest()
    {
        var expected = 123.45m;
        var name = "NAME";
        var target = new List<OFXProperty>();
        var actual = target.Add(expected, name);

        Assert.AreSame(target, actual, "Failed to return reference to self.");
        Assert.AreEqual(name, actual[0].Name);
        Assert.AreEqual(expected.ToString(), actual[0].Value);
    }

    [TestMethod]
    public void AddPositionAccountTypeTest()
    {
        var data = Position.PositionAccountTypes.CASH;
        var target = new List<OFXProperty>();
        var actual = target.Add(data);

        Assert.AreSame(target, actual, "Failed to return reference to self.");
        Assert.AreEqual("HELDINACCT", actual[0].Name);
        Assert.AreEqual(data.ToString(), actual[0].Value);
    }

    [TestMethod]
    public void AddPositionTypeTest()
    {
        var data = Position.PositionTypes.LONG;
        var target = new List<OFXProperty>();
        var actual = target.Add(data);

        Assert.AreSame(target, actual, "Failed to return reference to self.");
        Assert.AreEqual("POSTYPE", actual[0].Name);
        Assert.AreEqual(data.ToString(), actual[0].Value);
    }

    [TestMethod]
    public void AddPositionListTest()
    {
        var data = new PositionList();
        var id = "MSFT";
        var date = new DateTime(2024, 11, 05, 12, 37, 06, DateTimeKind.Utc);
        var position = new Position(id)
        {
            Memo = "memo",
            PriceDate = date,
            PositionType = Position.PositionTypes.LONG,
            MarketValue = 12345.67m,
            SecurityType = Security.SecurityTypes.OPTION,
            UnitPrice = 987.65m,
            SubAccountType = Position.PositionAccountTypes.MARGIN,
            Units = 112233m,
        };
        data.Items.Add(position);

        var target = new List<OFXProperty>();
        var actual = target.Add(data);

        Assert.AreSame(target, actual, "Failed to return reference to self.");
        Assert.AreEqual("INVPOSLIST", actual[0].Name);
    }
}
