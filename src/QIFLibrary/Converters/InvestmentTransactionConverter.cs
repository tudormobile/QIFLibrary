using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary.Converters;

/// <summary>
/// Provides mechanism for converting an OFX property to an Investment Transaction or Investment Transaction List.
/// </summary>
public class InvestmentTransactionConverter : PropertyConverterBase<InvestmentTransaction>, IPropertyConverter<InvestmentTransactionList>
{
    /// <summary>
    /// Convert to an investment transaction.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction if successful; otherwise (null).</returns>
    public override InvestmentTransaction? Convert(OFXProperty root)
    {
        if (root.HasChildren())
        {
            var secidProp = DigForProperty(root, "SECID");
            var invProp = DigForProperty(root, "INVTRAN");
            if (secidProp != null)
            {
                var secid = secidProp.Children["UNIQUEID"].Value;
                if (invProp != null && !string.IsNullOrEmpty(secid) && Enum.TryParse<InvestmentTransactionType>(root.Name, out var transactionType))
                {
                    var id = invProp.Children["FITID"].Value;
                    root = root.Children[0];
                    return new InvestmentTransaction(id, secid, transactionType)
                    {
                        DatePosted = invProp.Children["DTTRADE"].AsDate(),
                        Total = root.Children["TOTAL"].AsDecimal(),
                        UnitPrice = root.Children["UNITPRICE"].AsDecimal(),
                        Units = root.Children["UNITS"].AsDecimal(),
                    };

                }
            }
        }
        return null;    // unrecognized or unsupported type transaction type.
    }

    /// <summary>
    /// Convert to an investment transaction list.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new investment transaction list if successful; otherwise (null).</returns>
    InvestmentTransactionList? IPropertyConverter<InvestmentTransactionList>.Convert(OFXProperty root)
    {
        var p = DigForProperty(root, "INVTRANLIST");
        if (p != null)
        {
            var result = new InvestmentTransactionList()
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
    /// Converts OFX property to an investment transaction list.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>InvestmentTransactionList.</returns>
    public static InvestmentTransactionList? GetInvestmentTransactionList(this OFXPropertyConverter converter, OFXProperty root)
        => ((IPropertyConverter<InvestmentTransactionList>)new InvestmentTransactionConverter()).Convert(root);

    /// <summary>
    /// Converts OFX property to an investment transaction.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>InvestmentTransaction.</returns>
    public static InvestmentTransaction? GetInvestmentTransaction(this OFXPropertyConverter converter, OFXProperty root)
        => new InvestmentTransactionConverter().Convert(root);
}

