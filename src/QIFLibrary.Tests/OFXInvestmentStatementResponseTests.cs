using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests
{
    [TestClass]
    public class OFXInvestmentStatementResponseTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var target = new OFXInvestmentStatementResponse();

            Assert.IsEmpty(target.PositionList.Items);
            Assert.IsEmpty(target.TransactionList.Items);
            Assert.AreEqual(OFXCurrencyType.USD, target.Currency, "Default value required");
            Assert.IsNull(target.Investment401kBalance);

            Assert.AreEqual(string.Empty, target.Cookie);
            Assert.AreEqual(DateTime.Now.Year, target.Date.Year);

            Assert.IsNotNull(target.Account);

        }

        [TestMethod]
        public void CookieTest()
        {
            var content = @"
OFXHEADER:100
DATA:OFXSGML
VERSION:102
SECURITY:NONE
ENCODING:USASCII
CHARSET:1252
COMPRESSION:NONE
OLDFILEUID:NONE
NEWFILEUID:NONE

<OFX>
  <INVSTMTMSGSRSV1>
    <INVSTMTTRNRS>
      <TRNUID>123</TRNUID>
      <STATUS>
        <CODE>0</CODE>
        <SEVERITY>INFO</SEVERITY>
        <MESSAGE>SUCCESS</MESSAGE>
      </STATUS>
      <CLTCOOKIE>cookie
      <INVSTMTRS>
        <DTASOF>20240930000000.000</DTASOF>
        <CURDEF>CAD</CURDEF>
        <INVTRANLIST>
          <DTSTART>20240701000000.000</DTSTART>
          <DTEND>20240930000000.000</DTEND>
        </INVTRANLIST>
        <INVPOSLIST>
        </INVPOSLIST>
        <INV401KBAL>
          <TOTAL>1303080.04</TOTAL>
        </INV401KBAL>
      </INVSTMTRS>
    </INVSTMTTRNRS>
  </INVSTMTMSGSRSV1>
</OFX>";
            var doc = OFXDocument.Parse(content);
            var target = doc.MessageSets[0].Messages[0] as OFXInvestmentStatementResponse;
            Assert.IsNotNull(target);
            Assert.AreEqual("123", target.Id);
            Assert.AreEqual(OFXCurrencyType.CAD, target.Currency);
            Assert.AreEqual("cookie", target.Cookie);
            Assert.IsNotNull(target.Investment401kBalance);
        }
    }
}
