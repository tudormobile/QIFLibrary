using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters;

/// <summary>
/// Provides mechanism for converting an OFX property to a Security or Security List..
/// </summary>
public class SecurityConverter : PropertyConverterBase<Security>, IPropertyConverter<SecurityList>
{
    /// <summary>
    /// Convert to a security.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new security if successful; otherwise (null).</returns>
    public override Security? Convert(OFXProperty root)
    {
        var p = digForProperty(root, "SECINFO");
        if (p != null)
        {
            var propId = digForProperty(p, "UNIQUEID");
            if (propId != null)
            {
                return new Security(
                    propId.Value,
                    p.Children["TICKER"].Value,
                    p.Children["SECNAME"].Value,
                    p.Children["UNITPRICE"].AsDecimal(),
                    p.Children["DTASOF"].AsDate());
            }
        }
        return null;
    }

    /// <summary>
    /// Convert to a security list.
    /// </summary>
    /// <param name="root">Property to convert.</param>
    /// <returns>A new security list if successful; otherwise (null).</returns>
    SecurityList? IPropertyConverter<SecurityList>.Convert(OFXProperty root)
    {
        var p = digForProperty(root, "SECLIST");
        if (p != null)
        {
            var result = new SecurityList();
            foreach (var child in p.Children)
            {
                var security = Convert(child);
                if (security != null)
                {
                    result.Items.Add(security);
                }
            }
            return result;
        }
        return null;
    }
}
