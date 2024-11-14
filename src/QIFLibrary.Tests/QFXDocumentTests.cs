using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests;

[TestClass]
public class QFXDocumentTests
{
    [TestMethod]
    public void EmpowerExportTest()
    {
        var filename = Path.Combine("TestAssets", "empower.qfx");
        if (File.Exists(filename))
        {

            var target = OFXDocument.ParseFile(filename);

            var inst = new InstitutionConverter().Convert(target.MessageSets[0].Messages[0].AsProperty());
            Assert.IsNotNull(inst);
            Assert.AreEqual("empower.org", inst.Name);
            Assert.AreEqual("12345", inst.Id);

            var transactions = new OFXPropertyConverter().GetInvestmentTransactionList(target.MessageSets[1].Messages[0].AsProperty());
            var positions = new OFXPropertyConverter().GetPositionList(target.MessageSets[1].Messages[0].AsProperty());
            var securities = new OFXPropertyConverter().GetSecurityList(target.MessageSets[2].Messages[0].AsProperty());

            Assert.IsNotNull(positions);
            Assert.AreEqual(1, positions.Items.Count);

            Assert.IsNotNull(securities);
            Assert.AreEqual(1, securities.Items.Count);

            Assert.IsNotNull(transactions);
            Assert.AreEqual(3, transactions.Items.Count);

            Assert.AreEqual(11305.19m, transactions.Items[0].Total);
            Assert.AreEqual(598.365476m, transactions.Items[0].UnitPrice);
            Assert.AreEqual(-18.893453m, transactions.Items[0].Units);
            Assert.AreEqual(0m, transactions.Items[0].CommissionAndFees);
        }
    }

    [TestMethod]
    public void InvestmentStatementResponseTest()
    {
        var filename = Path.Combine("TestAssets", "empower.qfx");
        if (File.Exists(filename))
        {

            var target = OFXDocument.ParseFile(filename).MessageSets[1].Messages.First() as OFXInvestmentStatementResponse;

            Assert.IsNotNull(target);
            Assert.AreEqual(String.Empty, target.Cookie);
            Assert.AreEqual(new DateTime(2024, 09, 30, 00, 00, 00), target.Date.ToUniversalTime()); //20240930000000.000

            Assert.IsNotNull(target.Account);
            Assert.AreEqual("ACCTID1234567890", target.Account.AccountId);
            Assert.AreEqual(Account.AccountTypes.INVESTMENT, target.Account.AccountType);
            Assert.AreEqual("broker.com", target.Account.InstitutionId);

            Assert.AreEqual(1, target.PositionList.Items.Count);
            Assert.AreEqual(3, target.TransactionList.Items.Count);
        }
    }

}
