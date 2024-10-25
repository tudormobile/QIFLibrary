using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXDocumentTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var target = new OFXDocument();
        Assert.IsNotNull(target.Headers);
        Assert.AreEqual("application/x-ofx", OFXDocument.CONTENT_TYPE);
        Assert.AreEqual("ofx", OFXDocument.FILE_EXTENSION);
        Assert.AreEqual(0, target.MessageSets.Count, "Must contain initialized message list of zero message sets.");
        Assert.AreEqual("102", target.Version, "Default value for 'Version' property must be 102");
    }

    [TestMethod]
    public void VersionTest()
    {
        var expected = "1234";
        var target = new OFXDocument() { Version = expected };
        var actual = target.Version;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ParseTest1()
    {
        // minimal OFX file.
        var data = @"OFXHEADER:100
DATA:OFXSGML
VERSION:102
SECURITY:NONE
ENCODING:USASCII
CHARSET:1252
COMPRESSION:NONE
OLDFILEUID:NONE
NEWFILEUID:NONE

<OFX>
</OFX>
";
        var target = OFXDocument.Parse(data);
        Assert.AreEqual(0, target.MessageSets.Count, "There are no message sets.");

        Assert.AreEqual(9, target.Headers.Count, "Should have found 9 headers.");
        Assert.AreEqual("100", target.Headers["OFXHEADER"]);
        Assert.AreEqual("OFXSGML", target.Headers["DATA"]);
        Assert.AreEqual("102", target.Headers["VERSION"]);
        Assert.AreEqual("NONE", target.Headers["SECURITY"]);
        Assert.AreEqual("USASCII", target.Headers["ENCODING"]);
        Assert.AreEqual("1252", target.Headers["CHARSET"]);
        Assert.AreEqual("NONE", target.Headers["COMPRESSION"]);
        Assert.AreEqual("NONE", target.Headers["OLDFILEUID"]);
        Assert.AreEqual("NONE", target.Headers["NEWFILEUID"]);
    }

    [TestMethod]
    public void ParseTest2()
    {
        // minimal OFX file.
        var data = @"OFXHEADER:100
DATA:OFXSGML
VERSION:102
SECURITY:NONE
ENCODING:USASCII
CHARSET:1252
COMPRESSION:NONE
OLDFILEUID:NONE
NEWFILEUID:NONE

<OFX>
  <SIGNONMSGSRSV1>
    <SONRS>
      <STATUS>
        <CODE>0
        <SEVERITY>INFO
        <MESSAGE>Successful Sign On
      </STATUS>
        <DTSERVER>2024010203040506
        <LANGUAGE>ENG
        <DTPROFUP>20010918083000
      <FI>
        <ORG>broker.com
      </FI>
    </SONRS>
  </SIGNONMSGSRSV1>
</OFX>
";
        var target = OFXDocument.Parse(data);
        Assert.AreEqual(0, target.MessageSets.Count, "There are no message sets.");

        Assert.AreEqual(9, target.Headers.Count, "Should have found 9 headers.");
        Assert.AreEqual("100", target.Headers["OFXHEADER"]);
        Assert.AreEqual("OFXSGML", target.Headers["DATA"]);
        Assert.AreEqual("102", target.Headers["VERSION"]);
        Assert.AreEqual("NONE", target.Headers["SECURITY"]);
        Assert.AreEqual("USASCII", target.Headers["ENCODING"]);
        Assert.AreEqual("1252", target.Headers["CHARSET"]);
        Assert.AreEqual("NONE", target.Headers["COMPRESSION"]);
        Assert.AreEqual("NONE", target.Headers["OLDFILEUID"]);
        Assert.AreEqual("NONE", target.Headers["NEWFILEUID"]);
    }

}
