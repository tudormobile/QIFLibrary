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
        if (!File.Exists(filename)) return;

        var target = OFXDocument.ParseFile(filename);

        var inst = new InstitutionConverter().Convert(target.MessageSets[0].Messages[0].AsProperty());
        Assert.IsNotNull(inst);
        Assert.AreEqual("GWRS", inst.Name);
        Assert.AreEqual("3862", inst.Id);

        var transactions = new OFXPropertyConverter().GetInvestmentTransactionList(target.MessageSets[1].Messages[0].AsProperty());
        var positions = new OFXPropertyConverter().GetPositionList(target.MessageSets[1].Messages[0].AsProperty());
        var securities = new OFXPropertyConverter().GetSecurityList(target.MessageSets[2].Messages[0].AsProperty());

        Assert.IsNotNull(positions);
        Assert.AreEqual(1, positions.Items.Count);

        Assert.IsNotNull(securities);
        Assert.AreEqual(1, securities.Items.Count);

        Assert.IsNotNull(transactions);
        Assert.AreEqual(3, transactions.Items.Count);
    }

    [TestMethod]
    public void InvestmentStatementResponseTest()
    {
        var filename = Path.Combine("TestAssets", "empower.qfx");
        if (!File.Exists(filename)) return;

        var target = OFXDocument.ParseFile(filename).MessageSets[1].Messages.First() as OFXInvestmentStatementResponse;

        Assert.IsNotNull(target);
        Assert.AreEqual(String.Empty, target.Cookie);
        Assert.AreEqual(new DateTime(2024, 09, 30, 00, 00, 00), target.Date.ToUniversalTime()); //20240930000000.000

        Assert.IsNotNull(target.Account);
        Assert.AreEqual("20943567.150563-01", target.Account.AccountId);
        Assert.AreEqual(Account.AccountTypes.INVESTMENT, target.Account.AccountType);
        Assert.AreEqual("gwrs.com", target.Account.InstitutionId);

        Assert.AreEqual(1, target.PositionList.Items.Count);
        Assert.AreEqual(3, target.TransactionList.Items.Count);
    }

}
