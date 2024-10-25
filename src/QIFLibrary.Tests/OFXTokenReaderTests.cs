using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXTokenReaderTests
{
    [TestMethod]
    public void ReadTest()
    {
        var data = "<one><two><three>3<four>4  </four></two></one>";
        var target = new OFXTokenReader(new StringReader(data));
        var actual = target.ReadTokensAsync().ToBlockingEnumerable().ToArray();

        Assert.AreEqual(10, actual.Length);

        Assert.AreEqual(OFXTokenReader.OFXTokenType.StartTag, actual[0].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.StartTag, actual[1].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.StartTag, actual[2].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.Content, actual[3].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.StartTag, actual[4].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.Content, actual[5].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.EndTag, actual[6].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.EndTag, actual[7].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.EndTag, actual[8].TokenType);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.EndOfFile, actual[9].TokenType);

        Assert.AreEqual("one", actual[0].Data);
        Assert.AreEqual("one", actual[8].Data);

        Assert.AreEqual("3", actual[3].Data);
        Assert.AreEqual("4", actual[5].Data);
    }

    [TestMethod]
    public void PeekTest()
    {
        var data = @"
<one>
    <two>
        <three>3   
        <four>4</four>
    </two>
</one>
";
        var target = new OFXTokenReader(new StringReader(data));

        do
        {
            var expected = target.Peek();
            var actual = target.Peek();
            Assert.AreSame(expected, actual);
            actual = target.Read();
            Assert.AreEqual(expected.TokenType, actual.TokenType);
            Assert.AreEqual(expected.Data, actual.Data);
            Debug.WriteLine($"{actual.TokenType}, [{actual.Data}]");
        } while (target.Peek().TokenType != OFXTokenReader.OFXTokenType.EndOfFile);
    }
}
