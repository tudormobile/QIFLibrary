using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class CSVDocumentTests
{
    [TestMethod]
    public void ConstructorTest1()
    {
        var target = new CSVDocument();
        Assert.AreEqual(String.Empty, target.Name, "Must be initialize to <Empty> string.");
        Assert.AreEqual(0, target.Comments.Count, "Must initialize with zero comments.");
        Assert.AreEqual(0, target.Fields.Count, "Must be intiialized with zero fields.");
        Assert.AreEqual(0, target.Records.Count, "Must be initialized with zero records.");

        Assert.AreEqual("text/csv", CSVDocument.CONTENT_TYPE);
        Assert.AreEqual("csv", CSVDocument.FILE_EXTENSION);
    }

    [TestMethod]
    public void ParseFileTest1()
    {
        var expected = 3;
        var filename = Path.Combine("TestAssets", "sample1.csv");
        var target = CSVDocument.ParseFile(filename);
        var actual = target.Comments.Count;
        Assert.AreEqual(expected, actual);

        // Should have 7 records with 9 fields
        Assert.AreEqual(7, target.Records.Count, "Data should have 7 records.");
        Assert.AreEqual(9, target.Fields.Count, "Data should have 9 fields.");
    }

    [TestMethod]
    public void ParseFileTest2()
    {
        var expected = 0;
        var filename = Path.Combine("TestAssets", "sample2.csv");
        var target = CSVDocument.ParseFile(filename);
        var actual = target.Comments.Count;
        Assert.AreEqual(expected, actual);

        // Should have 7 records with 9 fields
        Assert.AreEqual(215, target.Records.Count, "Data should have 7 records.");
        Assert.AreEqual(11, target.Fields.Count, "Data should have 9 fields.");

        var d = target.Records[0]["Effective Date"];

        var success = target.Records[0].TryGetValue<DateTime>("Effective Date", out DateTime? dd);
    }

    [TestMethod]
    public void ParseFileTest3()
    {
        var expected = 0;
        var filename = Path.Combine("TestAssets", "sample3.csv");
        var target = CSVDocument.ParseFile(filename);
        var actual = target.Comments.Count;
        Assert.AreEqual(expected, actual);

        // Should have 7 records with 9 fields
        Assert.AreEqual(2, target.Records.Count, "Data should have 2 records.");
        Assert.AreEqual(0, target.Fields.Count, "Data should have ? fields.");

        Assert.AreEqual("1", target.Records[0][0]);
        Assert.AreEqual("2", target.Records[0][1]);
        Assert.AreEqual("3", target.Records[0][2]);
        Assert.AreEqual("4", target.Records[1][0]);
        Assert.AreEqual("5", target.Records[1][1]);
        Assert.AreEqual("6", target.Records[1][2]);

        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, target.Records[0].Values);
    }


}
