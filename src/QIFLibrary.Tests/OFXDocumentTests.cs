﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

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
    public void SaveSmallerTest()
    {
        // Test for Bug #29
        var filename = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        var largeText = new string('X', 1024);
        File.WriteAllText(filename, largeText);
        var target = new OFXDocument();
        target.Save(filename);
        var actual = File.ReadAllText(filename);
        Assert.IsTrue(actual.EndsWith("</OFX>"), "Failed to truncate old data from the file.");
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void SaveTest()
    {
        var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        var target = new OFXDocument();
        target.Save(path);
        if (File.Exists(path))
        {
            var actual = File.ReadAllText(path);
            File.Delete(path);
            Assert.IsTrue(actual.Contains("<OFX>"));
            Assert.IsTrue(actual.Contains("</OFX>"));
            return;
        }
        Assert.Fail("Failed to create file");
    }

    [TestMethod]
    public void DefaultHeadersTest()
    {
        var target = new OFXDocument();
        var expected = target.Version;

        Assert.AreSame(target, target.DefaultHeaders(), "Failed to return instance if self.");

        var actual = target.Headers["VERSION"];
        Assert.AreEqual(expected, actual);
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
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var target = OFXDocument.Parse(ms);

        Assert.AreEqual("102", target.Version);

        Assert.AreEqual(1, target.MessageSets.Count, "There should be exactly 1 message set.");
        Assert.AreEqual(1, target.MessageSets[0].Version);
        Assert.AreEqual(OFXMessageSetTypes.SIGNON, target.MessageSets[0].MessageSetType);
        Assert.AreEqual(OFXMessageDirection.RESPONSE, target.MessageSets[0].Direction);

        Assert.AreEqual(1, target.MessageSets[0].Messages.Count, "Should have found (1) message.");
        Assert.AreEqual("SONRS", target.MessageSets[0].Messages[0].Name);
        Assert.AreEqual("", target.MessageSets[0].Messages[0].Id, "No transaction Id in the data.");
        Assert.AreEqual("Successful Sign On", target.MessageSets[0].Messages[0].Status.Message);
        Assert.AreEqual(OFXStatus.StatusSeverity.INFO, target.MessageSets[0].Messages[0].Status.Severity);
        Assert.AreEqual(0, target.MessageSets[0].Messages[0].Status.Code);

        Assert.AreEqual(4, target.MessageSets[0].Messages[0].Properties.Count);

        Assert.AreEqual(9, target.Headers.Count, "Should have found 9 headers.");
        Assert.AreEqual("100", target.Headers.Version);
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
    public void QuickenQFXTest()
    {
        var filename = Path.Combine("TestAssets", "Quicken.qfx");
        if (File.Exists(filename))
        {

            var target = OFXDocument.ParseFile(filename);

            Assert.AreEqual(9, target.Headers.Count);
            Assert.AreEqual(2, target.MessageSets.Count);
            Assert.AreEqual(OFXMessageSetTypes.BANK, target.MessageSets[1].MessageSetType);
            Assert.AreEqual(1, target.MessageSets[1].Messages.Count);
            Assert.AreEqual("123", target.MessageSets[1].Messages[0].Id);
            Assert.AreEqual(4, target.MessageSets[1].Messages[0].Status.Code);
            Assert.AreEqual("", target.MessageSets[1].Messages[0].Status.Message);
            Assert.AreEqual(OFXStatus.StatusSeverity.INFO, target.MessageSets[1].Messages[0].Status.Severity);
            Assert.AreEqual("STMTRS", target.MessageSets[1].Messages[0].Properties[0].Name);
            Assert.AreEqual(5, target.MessageSets[1].Messages[0].Properties[0].Children.Count);

            // some converters?
            var actual = new InstitutionConverter().Convert(target.MessageSets[0].Messages[0].Properties[2]);

            var account = new OFXPropertyConverter().GetAccount(target.Root);

            Assert.IsNotNull(account);
            Assert.AreEqual("221379824", account.InstitutionId);
            Assert.AreEqual("67035K90", account.AccountId);
            Assert.AreEqual(Account.AccountTypes.CHECKING, account.AccountType);

            var transactions = new OFXPropertyConverter().GetTransactionList(target.Root);
            Assert.IsNotNull(transactions);
            Assert.AreEqual(7, transactions.Items.Count);
        }
    }

    [TestMethod]
    public void OFXInvestmentFileTest()
    {
        var filename = Path.Combine("TestAssets", "temp.ofx");
        if (File.Exists(filename))
        {

            var target = OFXDocument.ParseFile(filename);

            // find a position list
            var positions = new OFXPropertyConverter().GetPositionList(target.Root);

            var seclist = target.MessageSets[2].Messages[0].AsProperty();
            var securities = new OFXPropertyConverter().GetSecurityList(seclist);

            Assert.IsNotNull(positions);
            Assert.IsNotNull(securities);
            foreach (var item in positions.Items)
            {
                var id = item.SecurityId;
                Debug.WriteLine($"Position: {id} was found (of {positions.Items.Count} total):");

                var security = securities.Items.FirstOrDefault(x => x.Id == id);

                Assert.IsNotNull(security);
                Debug.WriteLine($">>>Security: '{security.Name}' ({security.Id}) was found with position {item.Units} shares at ${item.UnitPrice}");
            }
        }
    }
}