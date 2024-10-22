using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFRecordTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var date = DateTime.Now;
        var amount = 1.23m;
        var memo = "Some memo";
        var status = "Some Status";
        var actual = new QIFRecord(date, amount, memo, status);

        Assert.AreEqual(date, actual.Date);
        Assert.AreEqual(amount, actual.Amount);
        Assert.AreEqual(memo, actual.Memo);
        Assert.AreEqual(status, actual.Status);
    }
}
