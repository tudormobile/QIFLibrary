using System.Data;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Invetment position list property.
/// </summary>
public class OFXInvestmentPositionList : OFXProperty
{
    private PositionList _positions;

    /// <summary>
    /// Create an initialize a new instance.
    /// </summary>
    /// <param name="positions">Positions in the list.</param>
    public OFXInvestmentPositionList(PositionList positions) :base("INVPOSLIST")
    {
        _positions = positions;
    }

    /// <inheritdoc/>
    public override OFXPropertyCollection Children => generateProperties();

    private OFXPropertyCollection generateProperties()
    {
        var list = new OFXPropertyCollection();
        var converter = new PositionConverter();
        list.AddRange(_positions.Items.Select(position => converter.ToProperty(position)));
        return list;
    }
}
