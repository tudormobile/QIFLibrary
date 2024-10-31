using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFDocumentTests
{
    [TestMethod]
    public void ParseFileTest1()
    {
        var expected = QIFDocumentType.Investment;
        var expectedRecords = 26;
        var filename = Path.Combine("TestAssets", "retirement.qif");
        var actual = QIFDocument.ParseFile(filename);
        Assert.AreEqual(expected, actual.DataType);
        Assert.AreEqual(expectedRecords, actual.Records.Count, "Did NOT contain the expected record count.");
    }

    [TestMethod]
    public void ParseFileTest2()
    {
        var expected = QIFDocumentType.Investment;
        var expectedRecords = 29;
        var filename = Path.Combine("TestAssets", "bonds.qif");
        var actual = QIFDocument.ParseFile(filename);
        Assert.AreEqual(expected, actual.DataType);
        Assert.AreEqual(expectedRecords, actual.Records.Count, "Did NOT contain the expected record count.");
    }

    [TestMethod]
    public void ParseTest()
    {
        var expectedRecords = 0;
        var expected = QIFDocumentType.UNKNOWN;
        var data = "bad data";
        var actual = QIFDocument.Parse(data);
        Assert.AreEqual(expected, actual.DataType);
        Assert.AreEqual(expectedRecords, actual.Records.Count, "Did NOT contain the expected record count.");
    }

    [TestMethod]
    public void ParseStreamTest()
    {
        var expectedRecords = 1;
        var expected = QIFDocumentType.Investment;
        var data = @"!Type:Invst
D12/30'2023
T11,259.61
NShrsIn
YiShares Core Total Mkt ETF
I105.23
Q107
^
";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var actual = QIFDocument.Parse(ms);
        Assert.AreEqual(expected, actual.DataType);
        Assert.AreEqual(expectedRecords, actual.Records.Count, "Did NOT contain the expected record count.");
    }

    [TestMethod]
    public void AllDocumentTypesTest()
    {
        var data = new (string header, QIFDocumentType headerType)[]
        {
            ("!Type:Cash" , QIFDocumentType.Cash),
            ("!Type:Bank" , QIFDocumentType.Bank),
            ("!Type:CCard" , QIFDocumentType.CreditCard),
            ("!Type:Invst" , QIFDocumentType.Investment),
            ("!Type:Oth A" , QIFDocumentType.Asset),
            ("!Type:Oth L" , QIFDocumentType.Liability),
            ("!Type:Invoice" , QIFDocumentType.Invoice),
            ("!Type:Account" , QIFDocumentType.Account),
            ("!Type:Cat" , QIFDocumentType.Category),
            ("!Type:Class" , QIFDocumentType.Class),
            ("!Type:Memorized" , QIFDocumentType.Memorized),
            ("Bad Header", QIFDocumentType.UNKNOWN),
            (String.Empty, QIFDocumentType.UNKNOWN),
        };

        foreach (var testItem in data)
        {
            var actual = QIFDocument.Parse(testItem.header);
            Assert.AreEqual(testItem.headerType, actual.DataType, $"Failed to parse data header '{testItem.header}'.");
        }
    }

    [TestMethod]
    public void UnknownDocumentTypeTest()
    {
        var data = @"!Type:BadType
D5/6'2023
U11,259.61
NShrsIn
YiShares Core Total Mkt ETF
I105.23
CR
Mmemo
Q107
^
";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var actual = QIFDocument.Parse(ms);

        Assert.AreEqual(QIFDocumentType.UNKNOWN, actual.DataType, "Should have 'parsed' unknown document type.");
        Assert.AreEqual(1, actual.Records.Count, "Should have parsed one (1) unknown record type.");

        Assert.AreEqual(new DateTime(2023, 5, 6), actual.Records[0].Date);
        Assert.AreEqual("memo", actual.Records[0].Memo);
        Assert.AreEqual(11259.61m, actual.Records[0].Amount);
        Assert.AreEqual("R", actual.Records[0].Status);

    }

}
