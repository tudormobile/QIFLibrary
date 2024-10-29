using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIFLibrary.Tests.Entities;

[TestClass]
public class TransactionTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var target = new Transaction();

        Assert.IsFalse(string.IsNullOrWhiteSpace(target.Id));

        Assert.AreEqual(String.Empty, target.Name);
        Assert.AreEqual(String.Empty, target.Memo);
        Assert.AreEqual(Transaction.TransactionTypes.UNKNOWN, target.TransactionType);
        Assert.AreEqual(default(DateTime), target.DatePosted);
    }

    [TestMethod]
    public void PropertyTest()
    {
        var type = Transaction.TransactionTypes.FEE;
        var memo = "This is a memo";
        var name = "some name";
        var date = DateTime.Now;
        var id = "123ABC";
        var amount = 12.34m;

        var target = new Transaction()
        {
            Id = id,
            Name = name,
            Memo = memo,
            DatePosted = date,
            TransactionType = type,
            Amount = amount
        };

        Assert.AreEqual(id, target.Id);
        Assert.AreEqual(name, target.Name);
        Assert.AreEqual(memo, target.Memo);
        Assert.AreEqual(type, target.TransactionType);
        Assert.AreEqual(date, target.DatePosted);
        Assert.AreEqual(amount, target.Amount);
    }

    [TestMethod]
    public void TransactionCollectionTest()
    {
        var start = DateTime.Now;
        var end = DateTime.Now.AddDays(1);
        var target = new TransactionList()
        {
            Start = start,
            End = end
        };

        Assert.AreEqual(0, target.Items.Count);
        Assert.AreEqual(start, target.Start);
        Assert.AreEqual(end, target.End);
    }
}

