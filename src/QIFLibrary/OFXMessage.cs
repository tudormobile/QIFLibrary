using System.Numerics;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Message.
/// </summary>
public class OFXMessage
{
    /// <summary>
    /// Transaction Identifier.
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Message name (identifies type).
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Properties associated with the message.
    /// </summary>
    public IList<OFXProperty> Properties { get; } = [];

    /// <summary>
    /// Message status.
    /// </summary>
    public OFXStatus Status { get; set; } = new OFXStatus(OFXStatus.StatusSeverity.UNKNOWN);

    /// <summary>
    /// Representation of the message as a property.
    /// </summary>
    /// <returns></returns>
    public OFXProperty AsProperty()
    {
        var result = new OFXProperty(Name);
        result.Children.AddRange(Properties);
        return result;
    }

    /// <summary>
    /// Convert this instance to a string.
    /// </summary>
    /// <returns>String representation of the property.</returns>
    public override string ToString()
        => string.Concat(ToStrings());

    /// <summary>
    /// Convert this instance to strings.
    /// </summary>
    /// <returns>Enumeration of string reprenting the property.</returns>
    /// <remarks>
    /// This method is provided to allow serializers to customize their output streams.
    /// </remarks>
    public IEnumerable<String> ToStrings()
    {
        yield return $"<{Name.ToUpperInvariant()}>";

        // Severity (optional)
        if (Status.Severity != OFXStatus.StatusSeverity.UNKNOWN)
        {
            foreach (var s in Status.ToStrings()) yield return s;
        }

        // Identifier (optional)
        if (Id.Length > 0)
        {
            yield return $"<TRNUID>{Id}";
        }

        // Properties
        foreach (var p in Properties)
        {
            foreach (var s in p.ToStrings()) yield return s;
        }

        yield return $"</{Name.ToUpperInvariant()}>";

    }
}

