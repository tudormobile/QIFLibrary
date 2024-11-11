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
        if (!File.Exists(filename)) return;

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
        if (!File.Exists(filename)) return;

        var target = CSVDocument.ParseFile(filename);
        var actual = target.Comments.Count;
        Assert.AreEqual(expected, actual);

        // Should have 7 records with 9 fields
        Assert.AreEqual(215, target.Records.Count, "Data should have 7 records.");
        Assert.AreEqual(11, target.Fields.Count, "Data should have 9 fields.");

        var d = target.Records[0]["Effective Date"];

        var success = target.Records[0].TryGetValue<DateTime>("Effective Date", out DateTime dd);
    }

    [TestMethod]
    public void ParseFileTest3()
    {
        var expected = 0;
        var filename = Path.Combine("TestAssets", "sample3.csv");
        if (!File.Exists(filename)) return;

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

    // Edge cases
    [TestMethod]
    public void EmptyDataTest()
    {
        var data = new string[0];
        var target = CSVDocument.Parse(data);
        Assert.AreEqual(0, target.Records.Count);
        Assert.AreEqual(0, target.Comments.Count);
        Assert.AreEqual(0, target.Fields.Count);
        Assert.AreEqual(String.Empty, target.Name);
    }

    [TestMethod]
    public void SimpleDataTest()
    {
        var expected = new DateTime(1964, 3, 11);
        var name = "name";
        var data = "1,2,3/11/1964";
        var target = CSVDocument.Parse(data, name);
        Assert.AreEqual(1, target.Records.Count);
        Assert.AreEqual(0, target.Comments.Count);
        Assert.AreEqual(0, target.Fields.Count);
        Assert.AreEqual(name, target.Name);

        Assert.IsTrue(target.Records[0].TryGetValue(2, out DateTime actual));
        Assert.AreEqual(expected, actual);
        Assert.AreEqual("1", target.Records[0][0]);
        Assert.AreEqual("2", target.Records[0][1]);
    }

    [TestMethod]
    public void SimpleDataWithQuotesTest()
    {
        var expected = new DateTime(1964, 3, 11);
        var name = "name";
        var data = "\"1\",\"2\",\"3/11/1964\"";
        var target = CSVDocument.Parse(data, name);
        Assert.AreEqual(1, target.Records.Count);
        Assert.AreEqual(0, target.Comments.Count);
        Assert.AreEqual(0, target.Fields.Count);
        Assert.AreEqual(name, target.Name);

        Assert.IsTrue(target.Records[0].TryGetValue(2, out DateTime actual));
        Assert.AreEqual(expected, actual);
        Assert.IsTrue(target.Records[0].TryGetValue(0, out decimal value));
        Assert.AreEqual(1m, value);
        Assert.AreEqual("2", target.Records[0][1]);
    }

    [TestMethod]
    public void CompexDataWithQuotesTest()
    {
        var data = "\"one,two\",\"three\",\"";
        var target = CSVDocument.Parse(data);
        Assert.AreEqual("\"one", target.Records[0][0]);
        Assert.AreEqual("two\"", target.Records[0][1]);
        Assert.AreEqual("three", target.Records[0][2]);
        Assert.AreEqual("\"", target.Records[0][3]);
    }

    [TestMethod]
    public void NoDataTest()
    {
        var name = "name";
        string[] comments = { "one", "two" };
        var target = new CSVDocument(name, comments);

        Assert.AreEqual(2, target.Comments.Count);
        Assert.AreEqual("one", target.Comments[0]);
        Assert.AreEqual("two", target.Comments[1]);
        Assert.AreEqual(name, target.Name);

        Assert.AreEqual(0, target.Records.Count);
        Assert.AreEqual(0, target.Fields.Count);
    }

    [TestMethod]
    public void BadDataTest()
    {
        var data = @"one,two
one,two,three";
        var target = CSVDocument.Parse(data);
        Assert.AreEqual(2, target.Fields.Count);
    }

    [TestMethod]
    public void ChaseCCDocumemtTest()
    {
        var filename = Path.Combine("TestAssets", "chase.qfx");
        if (!File.Exists(filename)) return;
        var target = OFXDocument.ParseFile(filename);

        Assert.IsTrue(target.Headers.AsEnumerable().Any(), "Failed to read headers from malformed CHASE document.");
        Assert.IsTrue(target.MessageSets.AsEnumerable().Any(), "Failed to read transactions from malformed CHASE document.");
    }

}
