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
    public void TryForceReadHeadersTest1()
    {
        // This is the content that CHASE BANK returns (malformed)
        var content = @"
OFXHEADER:100
DATA:OFXSGML
VERSION:102
SECURITY:NONE
ENCODING:USASCII
CHARSET:1252
  : BAD HEADER
THIS:IS:ALSO:BAD
COMPRESSION:NONE
OLDFILEUID:NONE
NEWFILEUID:NONE
<OFX>
</OFX>
";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        // Try reading proper headers
        var result = target.TryReadHeader(out _);
        Assert.IsFalse(result); // Should have failed to read header when begings with blank line.
        var actual = target.TryForceReadHeaders(out var headers);
        Assert.IsTrue(actual, "Should have found malformed headers.");
        Assert.AreEqual(9, headers!.Length, "Should have found 9 headers.");
        Assert.IsTrue(target.TryMoveToStart(out var start, "OFX"), "Should have progressed to the OFX open tag.");
    }

    [TestMethod]
    public void TryForceReadHeadersTest2()
    {
        // This is the content that CHASE BANK returns (malformed)
        var content = @"</BAD><CONTENT></CONTENT>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        // Try reading proper headers
        var result = target.TryReadHeader(out _);
        Assert.IsFalse(result); // Should have failed to read header when begings with blank line.
        var actual = target.TryForceReadHeaders(out var headers);
        Assert.IsFalse(actual, "Should NOT have found malformed headers.");
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

    [TestMethod]
    public void TryMoveToStartWithMissingValueTest()
    {
        var content = @"</ONE></TWO></THREE>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);
        Assert.IsFalse(target.TryMoveToStart(out var start));
        var reader2 = new StringReader(content);
        target = new OFXReader(reader2);
        Assert.IsFalse(target.TryMoveToStart(out var start2, name: "name"));
    }

    [TestMethod]
    public void TryMoveToEndTest()
    {
        var content = @"<ONE><TWO></ONE>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        Assert.IsFalse(target.TryMoveToEnd(out var token, name: "TWO"));
        Assert.IsNotNull(token);
        Assert.AreEqual(OFXTokenReader.OFXTokenType.EndOfFile, token.TokenType, "Should have read to end of file.");
    }

    [TestMethod]
    public void TryReadMessageTest()
    {
        var content = @"<ONE><TWO><THREE></THREE></ONE>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsTrue(actual, "Should have found a message.");
        Assert.IsFalse(target.TryReadMessage(out _), "Should have been at EOF after consuming ONE messsage");

        target = new OFXReader(new StringReader(content));
        Assert.IsTrue(target.TryMoveToStart(out _, "ONE"), "Failed to move to start tag.");
        actual = target.TryReadMessage(out message);
        Assert.IsFalse(actual, "Should NOT have found a malformed message.");

        target = new OFXReader(new StringReader(content));
        actual = target.TryReadMessage(out message);
        Assert.IsTrue(actual, "Should have found a simple message.");
    }

    [TestMethod]
    public void TryReadMessageWithNoOuterTagTest()
    {
        var content = @"<ONE><TWO><THREE></THREE></NOTONE>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsFalse(actual, "Cannot be a message if no outer tag.");
    }

    [TestMethod]
    public void TryReadMessageWithStatusTest()
    {
        var content = @"<CCSTMTTRNRS><TRNUID>1<STATUS><CODE>5<SEVERITY>INFO<MESSAGE>Success</STATUS></CCSTMTTRNRS>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsTrue(actual, "Should have found a message.");
        Assert.IsNotNull(message);
        Assert.AreEqual(5, message.Status.Code);
    }

    [TestMethod]
    public void TryReadMessageWithBadStatusTest1()
    {
        var content = @"<CCSTMTTRNRS><TRNUID>1<STATUS><SEVERITY>NOTINFO</STATUS></CCSTMTTRNRS>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsTrue(actual, "Should have found a message.");
        Assert.IsNotNull(message);
        Assert.AreEqual(0, message.Status.Code);
        Assert.AreEqual("", message.Status.Message);
        Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, message.Status.Severity);
    }

    [TestMethod]
    public void TryReadMessageWithBadsStatusTest2()
    {
        var content = @"<CCSTMTTRNRS><TRNUID>1<STATUS><CODE>BADCODE<MESSAGE></STATUS></CCSTMTTRNRS>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsTrue(actual, "Should have found a message.");
        Assert.IsNotNull(message);
        Assert.AreEqual(0, message.Status.Code);
        Assert.AreEqual(OFXStatus.StatusSeverity.UNKNOWN, message.Status.Severity);
        Assert.AreEqual("", message.Status.Message);
    }

    [TestMethod]
    public void TryReadMessageSetTest()
    {
        var content = @"<SIGNONMSGSRSV1><SONRS><STATUS><CODE>0<SEVERITY>INFO</STATUS><DTSERVER>20241104120000[0:GMT]<LANGUAGE>ENG<FI><ORG>B1<FID>10898</FI><INTU.BID>10898</SONRS></SIGNONMSGSRSV1>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessageSet(out var message);
        Assert.IsTrue(actual);
        Assert.IsNotNull(message);
        Assert.AreEqual(OFXMessageSetTypes.SIGNON, message.MessageSetType);
    }

    [TestMethod]
    public void TryReadUnknownMessageSetTest()
    {
        var content = @"<xxxMSGSRQV1></xxxMSGSRQV1>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessageSet(out var message);
        Assert.IsTrue(actual);
        Assert.IsNotNull(message);
        Assert.AreEqual(OFXMessageSetTypes.UNKNOWN, message.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.REQUEST, message.Direction, "Should be request based on the name provided.");
    }

    [TestMethod]
    public void TryReadUnknownMessageSetTest2()
    {
        var content = @"<xxxMSGSRxV1></xxxMSGSRxV1>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessageSet(out var message);
        Assert.IsTrue(actual);
        Assert.IsNotNull(message);
        Assert.AreEqual(OFXMessageSetTypes.UNKNOWN, message.MessageSetType);
        Assert.AreEqual(OFXMessageDirection.UNKNOWN, message.Direction, "Should be unknown based on the name provided.");
    }

    [TestMethod]
    public void TryReadMessageSetWithBadNameTest()
    {
        var content = @"<xxxMSGSRQV></xxxMSGSRQV>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessageSet(out var message);
        Assert.IsTrue(actual);
        Assert.IsNotNull(message);
        Assert.AreEqual(0, message.Version);
    }

    [TestMethod]
    public void TryReadMessageWithBadPropertyTest()
    {
        var content = @"<xxxMSGSRQV><  ></inn  er></xxxMSGSRQV>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TryReadMessageWithMissingEndTagTest()
    {
        var content = @"<xxxMSGSRQV1><inner>data<inner2></another>";
        using var reader = new StringReader(content);
        var target = new OFXReader(reader);

        var actual = target.TryReadMessage(out var message);
        Assert.IsFalse(actual);
    }




}
