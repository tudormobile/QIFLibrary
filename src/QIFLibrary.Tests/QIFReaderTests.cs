using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFReaderTests
{
    [TestMethod]
    public async Task ReadRecordAsyncTest()
    {
        var expected = new DateTime(2023, 12, 30);
        var data = @"!Type:Invst
D12/30'2023
T11,259.61
NShrsIn
YiShares Core Total Mkt ETF
I105.23
Q107
^
";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));

        var target = QIFReader.FromStream(ms);
        var actual = await target.ReadRecordAsync();
        Assert.IsInstanceOfType<QIFInvestment>(actual);
        Assert.AreEqual(expected, actual.Date);
        target.Close();
    }

    [TestMethod]
    public async Task ReadRecordsAsyncTest()
    {
        var expected = 26;
        var filename = Path.Combine("TestAssets", "retirement.qif");
        var contents = File.ReadAllBytes(filename);
        var ms = new MemoryStream(contents);
        var actual = new List<QIFRecord>();
        var target = QIFReader.FromStream(ms);

        await foreach (var record in target.ReadRecordsAsync())
        {
            actual.Add(record);
        }

        target.Close();
        Assert.AreEqual(expected, actual.Count);
    }
    [TestMethod]
    public void BankTransactionsTest()
    {
        var expected = 33;
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(_bankData));
        var target = QIFReader.FromStream(ms);
        var actual = target.ReadRecordsAsync().ToBlockingEnumerable().Cast<QIFBankRecord>().ToList();
        target.Close();
        Assert.AreEqual(expected, actual.Count, "Failed to parse the correct number of records.");

        // Validate first record
        Assert.AreEqual(new DateTime(2024, 1, 2), actual[0].Date);
        Assert.AreEqual(-123456.78m, actual[0].Amount);
        Assert.AreEqual("X", actual[0].Status);
        Assert.AreEqual("Initial Balance", actual[0].Memo);
        Assert.AreEqual("Opening Balance", actual[0].Description);
        Assert.AreEqual("[Treasury Direct]", actual[0].Category);
        Assert.AreEqual("123", actual[0].CheckNo);
        Assert.AreEqual("Address", actual[0].Address);
        Assert.IsTrue(actual[0].Flagged);
    }

    [TestMethod]
    public void DateFormatsTest()
    {
        var expected = 6;
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(_dateData));
        var target = QIFReader.FromStream(ms);
        var actual = target.ReadRecordsAsync().ToBlockingEnumerable().Cast<QIFBankRecord>().ToList();
        target.Close();
        Assert.AreEqual(expected, actual.Count, "Failed to parse the correct number of records.");

        Assert.AreEqual(new DateTime(2024, 1, 2), actual[0].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), actual[1].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), actual[2].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), actual[3].Date);
        Assert.AreEqual(new DateTime(2006, 1, 2), actual[4].Date);
        Assert.AreEqual(new DateTime(2006, 1, 2), actual[5].Date);

    }

    [TestMethod]
    public void CategoryTest()
    {
        var expected = 1;
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(_catData));
        var target = QIFReader.FromStream(ms);
        var actual = target.ReadRecordsAsync().ToBlockingEnumerable().Cast<QIFCategoryRecord>().ToList();
        target.Close();
        Assert.AreEqual(expected, actual.Count, "Failed to parse the correct number of records.");

        Assert.AreEqual("Test Category", actual[0].Memo);
        Assert.AreEqual(1000m, actual[0].Budgeted);
        Assert.AreEqual("Investments:Stocks", actual[0].Category);
    }

    [TestMethod]
    public void AccountTest()
    {
        var expected = 3;
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(_accountData));
        var target = QIFReader.FromStream(ms);
        var actual = target.ReadRecordsAsync().ToBlockingEnumerable().ToList();
        target.Close();
        Assert.AreEqual(expected, actual.Count, "Failed to parse the correct number of records.");

        var actual1 = (actual[0] as QIFAccountRecord)!;
        var actual2 = (actual[1] as QIFAccountRecord)!;
        var actual3 = (actual[2] as QIFAccountRecord)!;

        // validate all properties
        Assert.AreEqual("Name of the account", actual1.Name);
        Assert.AreEqual("Description of the account", actual1.Description);
        Assert.AreEqual("Type of account", actual1.AccountType);
        Assert.AreEqual(12345m, actual1.Balance);
        Assert.AreEqual(5678.00m, actual1.CreditLimit);

        Assert.AreEqual("BofA", actual2.Name);
        Assert.AreEqual("Bank of America Checking", actual2.Description);
        Assert.AreEqual("Checking", actual2.AccountType);
        Assert.AreEqual(2433m, actual2.Balance);
        Assert.AreEqual(0m, actual2.CreditLimit);

        Assert.AreEqual("Discover", actual3.Name);
        Assert.AreEqual("Discover credit card", actual3.Description);
        Assert.AreEqual("", actual3.AccountType);
        Assert.AreEqual(0m, actual3.Balance);
        Assert.AreEqual(7000m, actual3.CreditLimit);

    }


    // Test Data below
    // N=name, D=description, T=type of account, $=balance, L=credit card limit
    private string _accountData = @"!Account
NName of the account
DDescription of the account
TType of account
$12,345
L5678
^
!Account
NBofA
DBank of America Checking
TChecking
$2,433
^
!Account
NDiscover
DDiscover credit card
L7000
^";

    private string _catData = @"!Type:Cat
MTest Category
B1000
LInvestments:Stocks
^
";
    private string _dateData = @"!Type:Bank
D1/2/2024
^
D1/2/24
^
D01/02/2024
^
D1/2'24
^
D1/2'6
^
D1/2/6
^
D14/15/16
^
";
    private string _bankData = @"!Type:Bank
D1/2'2024
T-123,456.78
CX
MInitial Balance
POpening Balance
L[Treasury Direct]
N123
AAddress
F
^
D1/2'2024
MTREASURY DIRECT - TREAS DRCT
T9,979.02
Pxfer to TreasuryDirect
L[Sunmark Checkin]
^
D1/16'2024
T-10,000.00
L[Sunmark Checkin]
^
D1/31'2024
T-10,000.00
L[Sunmark Checkin]
^
D2/15'2024
T-10,000.00
L[Sunmark Checkin]
^
D5/15'2024
T-29.11
L[Sunmark Checkin]
^
D12/30'2023
T0.00
CX
POpening Balance
L[Brokerage HSA (Schwab) (Cash)]
^
D12/30'2023
T353.56
PContributions
^
D1/5'2024
T284.65
PContributions
^
D1/19'2024
T1,184.62
PContributions
^
D1/22'2024
T0.09
Pinterest
^
D1/27'2024
Mtax year 2023
T500.00
PContributions
^
D2/2'2024
Mtax year 2023
T284.65
PContributions
^
D2/20'2024
Mtax year 2023
T284.62
PContributions
^
D2/20'2024
T0.11
Pinterest
^
D3/3'2024
Mtax year 2023
T284.64
PContributions
^
D3/21'2024
Mtax year 2023
T284.62
PContributions
^
D3/30'2024
T0.04
Pinterest
^
D4/3'2024
Mtax year 2023
T284.64
PContributions
^
D4/4'2024
T-460.00
Pxfer to HSA
^
D4/29'2024
T0.04
Pinterest
^
D5/16'2024
T-1,400.00
Pxfer to HSA
L[HSABank HSA]
^
D5/21'2024
MDIVIDEND
T0.01
Pinterest
^
D6/17'2024
MDIVIDEND
T0.04
Pinterest
^
D6/27'2024
T-1,300.00
L[HSABank HSA]
^
D7/16'2024
T-1,100.00
Pxfer to HSA
L[HSABank HSA]
^
D8/3'2024
MDIVIDEND
T0.18
Pinterest
^
D8/16'2024
MDIVIDEND
T0.03
Pinterest
^
D9/26'2024
T0.03
Pinterest
^
D9/27'2024
T-97.41
L[HSABank HSA]
^
D10/1'2024
T-37.24
L[HSABank HSA]
^
D10/10'2024
T-0.30
L[HSABank HSA]
^
D10/17'2024
T0.01
Pinterest
";  // NO final '^', still ok
}
