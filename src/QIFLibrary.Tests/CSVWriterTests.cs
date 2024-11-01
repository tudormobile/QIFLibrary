using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class CSVWriterTests
{
    [TestMethod]
    public void RoundTripTest()
    {
        var filename = Path.Combine("TestAssets", "sample1.csv");
        var doc = CSVDocument.ParseFile(filename);
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);

        var target = new CSVWriter(writer);
        target.Write(doc);
        writer.Flush();

        var actual = Encoding.UTF8.GetString(ms.ToArray()).Replace("\n", String.Empty).Replace("\r", string.Empty);
        var expected = File.ReadAllText(filename).Replace("\"", String.Empty).Replace("\n", String.Empty).Replace("\r", string.Empty);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void WriteCommentTest()
    {
        var data = "this, is a comment";
        var expected = "this  is a comment\r\n";

        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);

        var target = new CSVWriter(writer);
        target.WriteComment(data);
        writer.Flush();
        var actual = Encoding.UTF8.GetString(ms.ToArray());
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void WriteRecordTest()
    {
        CSVDocument.ICSVRecord record = new TestRecord();
        var expected = "\"12,34\",\"this\"is\" a test\",5\r\n";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);

        var target = new CSVWriter(writer);
        target.WriteRecord(record);
        writer.Flush();
        var actual = Encoding.UTF8.GetString(ms.ToArray());
        Assert.AreEqual(expected, actual);
    }

    public class TestRecord : CSVDocument.ICSVRecord
    {
        public string this[int index] => throw new NotImplementedException();

        public string this[string fieldName] => throw new NotImplementedException();

        public string[] Values => ["12,34","this\"is\" a test", "5"];

        public bool TryGetValue<T>(int index, out T? value) where T : IParsable<T>
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue<T>(string fieldName, out T? value) where T : IParsable<T>
        {
            throw new NotImplementedException();
        }
    }

}
