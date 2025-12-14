using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFRecordBuilderTests
{
    [TestMethod]
    public void Build_BasicRecord_ReturnsQIFRecord()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.CreditCard);
        var result = builder
            .Add("D12/31/2024")
            .Add("T-100.50")
            .Add("MTest memo")
            .Add("CX")
            .Build();

        Assert.IsNotNull(result);
        Assert.AreEqual(new DateTime(2024, 12, 31), result.Date);
        Assert.AreEqual(-100.50m, result.Amount);
        Assert.AreEqual("Test memo", result.Memo);
        Assert.AreEqual("X", result.Status);
    }

    [TestMethod]
    public void Build_AccountRecord_ReturnsQIFAccountRecord()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Account);
        var result = builder
            .Add("NChecking")
            .Add("DMain checking account")
            .Add("TBank")
            .Add("$5000.00")
            .Add("L10000.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFAccountRecord>(result);

        var accountRecord = (QIFAccountRecord)result;
        Assert.AreEqual("Checking", accountRecord.Name);
        Assert.AreEqual("Main checking account", accountRecord.Description);
        Assert.AreEqual("Bank", accountRecord.AccountType);
        Assert.AreEqual(5000.00m, accountRecord.Balance);
        Assert.AreEqual(10000.00m, accountRecord.CreditLimit);
    }

    [TestMethod]
    public void Build_AccountRecordWithExclamation_ReturnsQIFAccountRecord()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("!Account")
            .Add("NSavings")
            .Add("DMy savings")
            .Add("TBank")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFAccountRecord>(result);

        var accountRecord = (QIFAccountRecord)result;
        Assert.AreEqual("Savings", accountRecord.Name);
        Assert.AreEqual("My savings", accountRecord.Description);
    }

    [TestMethod]
    public void Build_BankRecord_ReturnsQIFBankRecord()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/15/2024")
            .Add("T-250.00")
            .Add("MPayment")
            .Add("CX")
            .Add("PJohn Doe")
            .Add("LUtilities:Electric")
            .Add("A123 Main St")
            .Add("N1001")
            .Add("F")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFBankRecord>(result);

        var bankRecord = (QIFBankRecord)result;
        Assert.AreEqual(new DateTime(2024, 1, 15), bankRecord.Date);
        Assert.AreEqual(-250.00m, bankRecord.Amount);
        Assert.AreEqual("Payment", bankRecord.Memo);
        Assert.AreEqual("X", bankRecord.Status);
        Assert.AreEqual("John Doe", bankRecord.Description);
        Assert.AreEqual("Utilities:Electric", bankRecord.Category);
        Assert.AreEqual("123 Main St", bankRecord.Address);
        Assert.AreEqual("1001", bankRecord.CheckNo);
        Assert.IsTrue(bankRecord.Flagged);
    }

    [TestMethod]
    public void Build_BankRecordWithoutFlag_FlaggedIsFalse()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/15/2024")
            .Add("T100.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFBankRecord>(result);

        var bankRecord = (QIFBankRecord)result;
        Assert.IsFalse(bankRecord.Flagged);
    }

    [TestMethod]
    public void Build_CategoryRecord_ReturnsQIFCategoryRecord()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Category);
        var result = builder
            .Add("LGroceries:Food")
            .Add("MMonthly food budget")
            .Add("B500.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFCategoryRecord>(result);

        var categoryRecord = (QIFCategoryRecord)result;
        Assert.AreEqual("Groceries:Food", categoryRecord.Category);
        Assert.AreEqual("Monthly food budget", categoryRecord.Memo);
        Assert.AreEqual(500.00m, categoryRecord.Budgeted);
    }

    [TestMethod]
    public void Build_InvestmentBuy_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D03/15/2024")
            .Add("NBuy")
            .Add("YMSFT")
            .Add("I350.75")
            .Add("Q10")
            .Add("O9.99")
            .Add("T3517.49")
            .Add("MMy memo")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(new DateTime(2024, 3, 15), investment.Date);
        Assert.AreEqual(QIFInvestmentType.Buy, investment.InvestmentAction);
        Assert.AreEqual("MSFT", investment.SecurityName);
        Assert.AreEqual(350.75m, investment.Price);
        Assert.AreEqual(10m, investment.Quantity);
        Assert.AreEqual(9.99m, investment.Commision);
        Assert.AreEqual(3517.49m, investment.Amount);
        Assert.AreEqual("My memo", investment.Memo);
    }

    [TestMethod]
    public void Build_InvestmentSell_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D04/20/2024")
            .Add("NSell")
            .Add("YAAPL")
            .Add("I180.50")
            .Add("Q5")
            .Add("O7.50")
            .Add("T895.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(QIFInvestmentType.Sell, investment.InvestmentAction);
        Assert.AreEqual("AAPL", investment.SecurityName);
    }

    [TestMethod]
    public void Build_InvestmentDiv_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D05/01/2024")
            .Add("NDiv")
            .Add("YTSLA")
            .Add("T50.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(QIFInvestmentType.Div, investment.InvestmentAction);
        Assert.AreEqual(50.00m, investment.Amount);
    }

    [TestMethod]
    public void Build_InvestmentIntInc_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D06/01/2024")
            .Add("NIntInc")
            .Add("YBOND")
            .Add("T25.00")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(QIFInvestmentType.IntInc, investment.InvestmentAction);
    }

    [TestMethod]
    public void Build_InvestmentShrsIn_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D07/01/2024")
            .Add("NShrsIn")
            .Add("YGOOG")
            .Add("Q100")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(QIFInvestmentType.ShrsIn, investment.InvestmentAction);
        Assert.AreEqual(100m, investment.Quantity);
    }

    [TestMethod]
    public void Build_InvestmentShrsOut_ReturnsQIFInvestment()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D08/01/2024")
            .Add("NShrsOut")
            .Add("YAMZN")
            .Add("Q50")
            .Build();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual(QIFInvestmentType.ShrsOut, investment.InvestmentAction);
    }

    [TestMethod]
    public void Build_InvestmentBuyX_ThrowsNotSupportedException()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);

        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            builder
                .Add("NBuyX")
                .Add("YMSFT")
                .Build();
        });
    }

    [TestMethod]
    public void Build_InvestmentSellX_ThrowsNotSupportedException()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);

        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            builder
                .Add("NSellX")
                .Build();
        });
    }

    [TestMethod]
    public void Build_InvestmentInvalidAction_ReturnsEmpty()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("NInvalidAction")
            .Build();

        Assert.IsNotNull(result);
        Assert.AreEqual(QIFInvestment.Empty.Date, ((QIFInvestment)result).Date);
    }

    [TestMethod]
    public void Add_AllowsDuplicates_LastOneWins()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/01/2024")
            .Add("T100.00")
            .Add("T200.00")
            .Add("MFirst memo")
            .Add("MSecond memo")
            .Build();

        Assert.AreEqual(200.00m, result.Amount);
        Assert.AreEqual("Second memo", result.Memo);
    }

    [TestMethod]
    public void Clear_RemovesAllData()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        builder
            .Add("D01/01/2024")
            .Add("T100.00")
            .Clear()
            .Add("D02/01/2024")
            .Add("T200.00");

        var result = builder.Build();

        Assert.AreEqual(new DateTime(2024, 2, 1), result.Date);
        Assert.AreEqual(200.00m, result.Amount);
    }

    [TestMethod]
    public void Build_DateWithApostrophes_ParsesCorrectly()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D12'31'2024")
            .Add("T100.00")
            .Build();

        Assert.AreEqual(new DateTime(2024, 12, 31), result.Date);
    }

    [TestMethod]
    public void Build_InvalidDate_ReturnsMinValue()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("DInvalidDate")
            .Add("T100.00")
            .Build();

        Assert.AreEqual(DateTime.MinValue, result.Date);
    }

    [TestMethod]
    public void Build_InvalidDecimal_ReturnsZero()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/01/2024")
            .Add("TNotANumber")
            .Build();

        Assert.AreEqual(0m, result.Amount);
    }

    [TestMethod]
    public void Build_AmountFromUField_UsesUWhenTMissing()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/01/2024")
            .Add("U150.75")
            .Build();

        Assert.AreEqual(150.75m, result.Amount);
    }

    [TestMethod]
    public void Build_AmountFromTField_PrefersTOverU()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/01/2024")
            .Add("T100.00")
            .Add("U150.00")
            .Build();

        Assert.AreEqual(100.00m, result.Amount);
    }

    [TestMethod]
    public void Build_MemoWithTrailingSpaces_TrimmedCorrectly()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder
            .Add("D01/01/2024")
            .Add("T100.00")
            .Add("MTest memo   ")
            .Build();

        Assert.AreEqual("Test memo", result.Memo);
    }

    [TestMethod]
    public void Build_EmptyFields_ReturnsDefaults()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var result = builder.Build();

        Assert.IsNotNull(result);
        Assert.AreEqual(DateTime.MinValue, result.Date);
        Assert.AreEqual(0m, result.Amount);
        Assert.AreEqual(String.Empty, result.Memo);
        Assert.AreEqual(String.Empty, result.Status);
    }

    [TestMethod]
    public void Build_InvestmentWithAllFields_PopulatesCorrectly()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Investment);
        var result = builder
            .Add("D03/15/2024")
            .Add("NBuy")
            .Add("YMSFT")
            .Add("I350.75")
            .Add("Q10")
            .Add("O9.99")
            .Add("T3517.49")
            .Add("MMy memo")
            .Add("CX")
            //.Add("N1234") // investment doesn't have Check field
            .Add("PBroker Name")
            .Add("A123 Wall St")
            .Add("LInvestments")
            .Add("$100.00")
            .Build();

        Assert.IsInstanceOfType<QIFInvestment>(result);

        var investment = (QIFInvestment)result;
        Assert.AreEqual("X", investment.Status);
        Assert.AreEqual("Broker Name", investment.Payee);
        Assert.AreEqual("123 Wall St", investment.Address);
        Assert.AreEqual("Investments", investment.Category);
        Assert.AreEqual(100.00m, investment.SplitAmount);
    }

    [TestMethod]
    public void Build_CategoryWithNoMemo_ReturnsEmptyMemo()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Category);
        var result = builder
            .Add("LUtilities")
            .Build();

        Assert.IsInstanceOfType<QIFCategoryRecord>(result);

        var category = (QIFCategoryRecord)result;
        Assert.AreEqual("Utilities", category.Category);
        Assert.AreEqual(String.Empty, category.Memo);
        Assert.AreEqual(0m, category.Budgeted);
    }

    [TestMethod]
    public void Add_ReturnsBuilder_ForFluentInterface()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var returnedBuilder = builder.Add("D01/01/2024");

        Assert.AreSame(builder, returnedBuilder);
    }

    [TestMethod]
    public void Clear_ReturnsBuilder_ForFluentInterface()
    {
        var builder = new QIFRecordBuilder(QIFDocumentType.Bank);
        var returnedBuilder = builder.Clear();

        Assert.AreSame(builder, returnedBuilder);
    }
}
