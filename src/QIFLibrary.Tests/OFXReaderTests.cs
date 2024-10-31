using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXReaderTests
{
    [TestMethod]
    public void ConstructorTests()
    {
        var target = new OFXReader(new StringReader(String.Empty));
        // how to validate?
    }

    [TestMethod]
    public void TryReadHeaderTest1()
    {
        var expected = true;
        using var reader = new StringReader("key:value");
        var target = new OFXReader(reader);
        var actual = target.TryReadHeader(out var header);
        Assert.AreEqual(expected, actual);
        Assert.AreEqual("key", header.Key);
        Assert.AreEqual("value", header.Value);
    }

    [TestMethod]
    public void TryReadHeaderTest2()
    {
        var expected = false;
        string[] data = [
            "key value",
            "key=value",
            ":",
            " :  ",
            "",
            "\t \r\n",
            ];
        foreach (var item in data)
        {
            using var reader = new StringReader(item);
            var target = new OFXReader(reader);
            var actual = target.TryReadHeader(out var header);
            Assert.AreEqual(expected, actual);
        }
    }

}
