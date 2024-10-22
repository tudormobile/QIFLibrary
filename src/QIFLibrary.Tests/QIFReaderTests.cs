using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
