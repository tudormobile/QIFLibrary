using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters;

/// <summary>
/// Provides mechanism for converting an OFX property to a Transaction or Transaction List.
/// </summary>
public class TransactionConverter : PropertyConverterBase<Transaction>, IPropertyConverter<TransactionList>
{
    /// <summary>
    /// Convert to a transaction.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction if successful; otherwise (null).</returns>
    public override Transaction? Convert(OFXProperty root)
    {
        var p = digForProperty(root, "STMTTRN");
        if (p != null)
        {
            var propTrans = p.Children["FITID"].Value;
            var tid = propTrans.Length > 0 ? propTrans : new Guid().ToString();
            return new Transaction(tid)
            {
                Amount = p.Children["TRNAMT"].AsDecimal(),
                DatePosted = p.Children["DTPOSTED"].AsDate(),
                Memo = p.Children["MEMO"].Value,
                Name = p.Children["NAME"].Value,
                TransactionType = (Transaction.TransactionTypes)p.Children["TRNTYPE"].AsTransactionType(),
            };
        }
        return null;
    }

    /// <summary>
    /// Convert to a transaction list.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction list if successful; otherwise (null).</returns>
    TransactionList? IPropertyConverter<TransactionList>.Convert(OFXProperty root)
    {
        var p = digForProperty(root, "BANKTRANLIST");
        if (p != null)
        {
            var result = new TransactionList()
            {
                Start = p.Children["DTSTART"].AsDate(),
                End = p.Children["DTEND"].AsDate(),
            };

            foreach (var child in p.Children)
            {
                var transaction = Convert(child);
                if (transaction != null)
                {
                    result.Items.Add(transaction);
                }
            }
            return result;
        }
        return null;
    }
}

/// <inheritdoc/>
public static partial class OFXPropertyConverterExtensions
{
    /// <summary>
    /// Converts OFX property to a transaction list.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>TransactionList.</returns>
    public static TransactionList? GetTransactionList(this OFXPropertyConverter converter, OFXProperty root)
        => ((IPropertyConverter<TransactionList>)new TransactionConverter()).Convert(root);

    /// <summary>
    /// Converts OFX property to a transaction.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>Transaction.</returns>
    public static Transaction? GetTransaction(this OFXPropertyConverter converter, OFXProperty root)
        => new TransactionConverter().Convert(root);
}

