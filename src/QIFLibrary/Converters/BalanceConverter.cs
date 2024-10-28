using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters;

/// <inheritdoc/>
public class BalanceConverter : PropertyConverterBase<Nullable<Balance>>
{
    /// <summary>
    /// Key property for this entity.
    /// </summary>
    public static string KEY = "LEDGERBAL|AVAILBAL";

    /// <inheritdoc/>
    public override Nullable<Balance> Convert(OFXProperty root)
    {
        foreach (var key in KEY.Split('|'))
        {
            var p = digForProperty(root, key);
            if (p != null)
            {
                var d = p.Children["DTASOF"];
                return new Balance(p.Children["BALAMT"].AsDecimal(), d.Value.Length > 0 ? d.AsDate() : null);
            }
        }
        return default;
    }
}

/// <inheritdoc/>
public static partial class OFXPropertyConverterExtensions
{
    /// <summary>
    /// Converts OFX property to balances.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>Balances.</returns>
    public static Balance? GetBalance(this OFXPropertyConverter converter, OFXProperty root)
        => new BalanceConverter().Convert(root);
}
