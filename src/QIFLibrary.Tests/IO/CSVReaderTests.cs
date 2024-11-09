using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.IO;

namespace QIFLibrary.Tests.IO;

[TestClass]
public class CSVReaderTests
{
    [TestMethod]
    public void FromStreamTest1()
    {
        var data = @"one,two,three
1,2,3
4,5,6";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var target = CSVReader.FromStream(ms, hasHeader: true);
        Assert.AreEqual(target.ReadComments().Count(), 0);
        Assert.AreEqual(3, target.ReadHeader().Count());
        Assert.AreEqual(2, target.ReadRecords().Count());

    }

    [TestMethod]
    public void FromStreamTest2()
    {
        var data = @"one,two,three
1,2,3
4,5,6";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var target = CSVReader.FromStream(ms, hasHeader: true);
        Assert.AreEqual(target.ReadComments().Count(), 0);
        Assert.AreEqual(2, target.ReadRecords().Count());
    }

    [TestMethod]
    public void ComplexRecordTest()
    {
        var data = "\"A,B,C,D,E,F,G \"\"is\"\" the alphabet start\",1,,2,3,\"4\"";
        var reader = new StringReader(data);
        var target = new CSVReader(reader);

        var result = target.ReadRecord();

        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Values.Length);
        Assert.AreEqual("\"A,B,C,D,E,F,G \"\"is\"\" the alphabet start\"", result.Values[0]);
        Assert.AreEqual("1", result.Values[1]);
        Assert.AreEqual("", result.Values[2]);
        Assert.AreEqual("2", result.Values[3]);
        Assert.AreEqual("3", result.Values[4]);
        Assert.AreEqual("\"4\"", result.Values[5]);
    }

    [TestMethod]
    public void EmptyRecordTest()
    {
        var data = "";
        var reader = new StringReader(data);
        var target = new CSVReader(reader);

        Assert.AreEqual(target.ReadComments().Count(), 0);
        Assert.AreEqual(0, target.ReadHeader().Count());
        Assert.AreEqual(0, target.ReadRecords().Count());
    }

    [TestMethod]
    public void HasCommentsAndHeader()
    {
        var data = @"This is a comment
And this is nother comment line
f1,f2,f3
1,2,3
";
        var reader = new StringReader(data);
        var target = new CSVReader(reader);

        Assert.AreEqual(2, target.ReadComments().Count());
        Assert.AreEqual(3, target.ReadHeader().Count());
        Assert.AreEqual(1, target.ReadRecords().Count());

    }

    [TestMethod]
    public async Task ReadRecordAsyncTest()
    {
        var data = "1,2,3,4,5";
        var reader = new StringReader(data);
        var target = new CSVReader(reader);

        var actual = await target.ReadRecordAsync();

        Assert.IsNotNull(actual);
        Assert.AreEqual("1", actual[0]);
        Assert.AreEqual("5", actual[4]);
        Assert.IsTrue(actual.TryGetValue(0, out decimal val));
        Assert.AreEqual(1m, val);
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void ReadRecordsAsyncTest()
    {
        var data = @"name,number
joe,1
mike,2";
        var reader = new StringReader(data);
        var target = new CSVReader(reader, hasHeader: true);

        var actual = target.ReadRecordsAsync().ToBlockingEnumerable().ToList();
        Assert.AreEqual(2, actual.Count);
        Assert.AreEqual("joe", actual[0]["name"]);
        Assert.AreEqual("1", actual[0]["number"]);

        Assert.IsTrue(actual[1].TryGetValue("number", out int ival));
        Assert.AreEqual(2, ival);
        // is this the behavior we want?
        try
        {
            Assert.IsFalse(actual[0].TryGetValue("missing", out string? val));
            Assert.Fail("Should have thrown index out of range exception.");
        }
        catch (IndexOutOfRangeException) { }
    }

}
