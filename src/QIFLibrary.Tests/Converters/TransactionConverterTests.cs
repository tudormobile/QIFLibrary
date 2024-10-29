using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace QIFLibrary.Tests.Converters;

[TestClass]
public class TransactionConverterTests
{
    [TestMethod]
    public void ConvertTest()
    {
        var root = new OFXProperty("BANKTRANLIST");
        root.Children.Add(new OFXProperty("DTSTART", "20241021104315"));
        root.Children.Add(new OFXProperty("DTEND", "20241021104316"));
        var transaction = new OFXProperty("STMTTRN");

        transaction.Children.Add(new OFXProperty("FITID", "123"));
        transaction.Children.Add(new OFXProperty("TRNAMT", "-384.87"));
        transaction.Children.Add(new OFXProperty("NAME", "ACH Withdrawal"));
        transaction.Children.Add(new OFXProperty("MEMO", "VIKING RIVER CRU - 8182271234"));
        transaction.Children.Add(new OFXProperty("DTPOSTED", "20241004000000"));
        transaction.Children.Add(new OFXProperty("TRNTYPE", "CASH"));

        root.Children.Add(transaction);

        var target = new TransactionConverter();
        var actual = target.Convert(root);

        Assert.IsNotNull(actual);
        Assert.AreEqual("123", actual.Id);
        Assert.AreEqual(-384.87m, actual.Amount);
        Assert.AreEqual("VIKING RIVER CRU - 8182271234", actual.Memo);
        Assert.AreEqual("ACH Withdrawal", actual.Name);
        Assert.AreEqual(new DateTime(2024, 10, 4, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(), actual.DatePosted);
        Assert.AreEqual(Transaction.TransactionTypes.CASH, actual.TransactionType);

        var list = (target as IPropertyConverter<TransactionList>).Convert(root);
        Assert.AreEqual(1, list.Items.Count);
        Assert.AreEqual(new DateTime(2024, 10, 21, 10, 43, 15, DateTimeKind.Utc).ToLocalTime(), list.Start);
        Assert.AreEqual(new DateTime(2024, 10, 21, 10, 43, 16, DateTimeKind.Utc).ToLocalTime(), list.End);
    }

    [TestMethod]
    public void ConvertWithNoIdTest()
    {
        var root = new OFXProperty("BANKTRANLIST");
        root.Children.Add(new OFXProperty("DTSTART", "20241021104315"));
        root.Children.Add(new OFXProperty("DTEND", "20241021104315"));
        var transaction = new OFXProperty("STMTTRN");

        transaction.Children.Add(new OFXProperty("TRNAMT", "-384.87"));
        transaction.Children.Add(new OFXProperty("NAME", "ACH Withdrawal"));
        transaction.Children.Add(new OFXProperty("MEMO", "VIKING RIVER CRU - 8182271234"));
        transaction.Children.Add(new OFXProperty("DTPOSTED", "20241004180000"));
        transaction.Children.Add(new OFXProperty("TRNTYPE", "CASH"));

        root.Children.Add(transaction);

        var target = new TransactionConverter();
        var actual = target.Convert(root);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Id.Length > 0);
        Assert.AreEqual(-384.87m, actual.Amount);
        Assert.AreEqual("VIKING RIVER CRU - 8182271234", actual.Memo);
        Assert.AreEqual("ACH Withdrawal", actual.Name);
        Assert.AreEqual(new DateTime(2024, 10, 4, 18, 0, 0, DateTimeKind.Utc).ToLocalTime(), actual.DatePosted);
        Assert.AreEqual(Transaction.TransactionTypes.CASH, actual.TransactionType);
    }

    [TestMethod]
    public void BadConvertTest()
    {
        Assert.IsNull(new TransactionConverter().Convert(new OFXProperty("bad name")));
        Assert.IsNull((new TransactionConverter() as IPropertyConverter<TransactionList>).Convert(new OFXProperty("Bad list")));
    }

    [TestMethod]
    public void ConverterTest()
    {
        var root = new OFXProperty("Bad Root");

        var target = new OFXPropertyConverter(
            );
        var actual = target.GetTransaction(root);

        Assert.IsNull(actual);
        Assert.IsNull(target.GetTransactionList(root));
    }

}