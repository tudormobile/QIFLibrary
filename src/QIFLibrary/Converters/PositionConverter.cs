using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters;

/// <summary>
/// Provides mechanism for converting an OFX property to a Position or Position List.
/// </summary>
public class PositionConverter : PropertyConverterBase<Position>, IPropertyConverter<PositionList>
{
    /// <summary>
    /// Convert to a position.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction if successful; otherwise (null).</returns>
    public override Position? Convert(OFXProperty root)
    {
        //var wrapper = "POSMF|POSSTOCK|POSDEBT|POSOPT|POSOTHER";
        var wrapper = new Dictionary<string, Security.SecurityTypes>()
        {
            {"POSMF", Security.SecurityTypes.MUTUALFUND },
            {"POSSTOCK", Security.SecurityTypes.STOCK },
            {"POSDEBT", Security.SecurityTypes.DEBT },
            {"POSOPT", Security.SecurityTypes.OPTION },
            {"POSOTHER", Security.SecurityTypes.OTHER },
        };

        foreach (var item in wrapper)
        {
            var p = digForProperty(root, item.Key);
            var securityType = p == null ? Security.SecurityTypes.UNKNOWN : item.Value;
            if (p != null)
            {

                // Now dig out the investment position
                p = digForProperty(p, "INVPOS");
                if (p != null)
                {
                    // Required to have a security Id.
                    var propSecurityId = p.Children["SECID"];
                    if (propSecurityId != null)
                    {
                        var propId = digForProperty(p, "UNIQUEID");
                        if (propId != null)
                        {
                            // valid postion (even if everything else is missing)
                            var result = new Position(propId.Value)
                            {
                                SecurityType = securityType,
                                PositionType = (Position.PositionTypes)p.Children["POSTYPE"].AsPositionType(),
                                SubAccountType = (Position.PositionAccountTypes)p.Children["HELDINACCT"].AsPositionAccountType(),
                                Units = p.Children["UNITS"].AsDecimal(),
                                UnitPrice = p.Children["UNITPRICE"].AsDecimal(),
                                MarketValue = p.Children["MKTVAL"].AsDecimal(),
                                PriceDate = p.Children["DTPRICEASOF"].AsDate(),
                                Memo = p.Children["MEMO"].Value,
                            };

                            return result;
                        }
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Convert to a position list.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction list if successful; otherwise (null).</returns>
    PositionList? IPropertyConverter<PositionList>.Convert(OFXProperty root)
    {
        var p = digForProperty(root, "INVPOSLIST");
        if (p != null)
        {
            var result = new PositionList();

            foreach (var child in p.Children)
            {
                var position = Convert(child);
                if (position != null)
                {
                    result.Items.Add(position);
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
    /// Converts OFX property to a position list.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>A PositionList.</returns>
    public static PositionList? GetPositionList(this OFXPropertyConverter converter, OFXProperty root)
        => ((IPropertyConverter<PositionList>)new PositionConverter()).Convert(root);

    /// <summary>
    /// Converts OFX property to a position.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>Position.</returns>
    public static Position? GetPosition(this OFXPropertyConverter converter, OFXProperty root)
        => new PositionConverter().Convert(root);
}

