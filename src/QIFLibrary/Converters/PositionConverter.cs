using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary.Converters;

/// <summary>
/// Provides mechanism for converting an OFX property to a Position or Position List.
/// </summary>
public class PositionConverter : PropertyConverterBase<Position>, IPropertyConverter<PositionList>
{
    private readonly Dictionary<string, Security.SecurityTypes> _wrapper = new()
        {
            {"POSMF", Security.SecurityTypes.MUTUALFUND },
            {"POSSTOCK", Security.SecurityTypes.STOCK },
            {"POSDEBT", Security.SecurityTypes.DEBT },
            {"POSOPT", Security.SecurityTypes.OPTION },
            {"POSOTHER", Security.SecurityTypes.OTHER },
        };

    /// <summary>
    /// Convert to a position.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction if successful; otherwise (null).</returns>
    public override Position? Convert(OFXProperty root)
    {
        //var wrapper = "POSMF|POSSTOCK|POSDEBT|POSOPT|POSOTHER";

        foreach (var item in _wrapper)
        {
            var p = DigForProperty(root, item.Key);
            var securityType = p == null ? Security.SecurityTypes.UNKNOWN : item.Value;
            if (p != null)
            {

                // Now dig out the investment position
                p = DigForProperty(p, "INVPOS");
                if (p != null)
                {
                    // Required to have a security Id.
                    var propSecurityId = p.Children["SECID"];
                    if (propSecurityId != null)
                    {
                        var propId = DigForProperty(p, "UNIQUEID");
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
    /// Converty position to an OFX property.
    /// </summary>
    /// <param name="position">Position to convert.</param>
    /// <returns>OFX Property representing the position.</returns>
    public OFXProperty ToProperty(Position position)
    {
        var name = _wrapper.FirstOrDefault(kvp => kvp.Value == position.SecurityType).Key;
        var result = new OFXProperty(name);
        var pos = new OFXProperty("INVPOS");
        var id = new OFXProperty("SECID");
        id.Children.Add(new OFXProperty("UNIQUEID", position.SecurityId));
        id.Children.Add(new OFXProperty("UNIQUEIDTYPE", Security.SecurityIdTypes.TICKER.ToString()));
        pos.Children.Add(id);

        pos.Children
            .Add(position.SubAccountType)
            .Add(position.PositionType)
            .Add(position.Units, "UNITS")
            .Add(position.UnitPrice, "UNITPRICE")
            .Add(position.MarketValue, "MKTVAL", MidpointRounding.ToEven)
            .Add(position.PriceDate, "PRICEASOF")
            .Add(new OFXProperty("MEMO", position.Memo));

        result.Children.Add(pos);
        return result;
    }

    /// <summary>
    /// Convert to a position list.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new transaction list if successful; otherwise (null).</returns>
    PositionList? IPropertyConverter<PositionList>.Convert(OFXProperty root)
    {
        var p = DigForProperty(root, "INVPOSLIST");
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

