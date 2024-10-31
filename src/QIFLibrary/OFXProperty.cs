using System.Text;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Property (name, value pair).
/// </summary>
public class OFXProperty
{
    /// <summary>
    /// Property name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Property value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Child Properties.
    /// </summary>
    public OFXPropertyCollection Children { get; } = [];

    /// <summary>
    /// Create and initialize a new instance.
    /// </summary>
    /// <param name="name">Property name.</param>
    /// <param name="value">Property value.</param>
    public OFXProperty(string name, string value = "")
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Convert this property to a string.
    /// </summary>
    /// <returns>String representation of the property.</returns>
    public override string ToString()
    {
        if (Children.Count > 0)
        {
            var result = new StringBuilder();
            result.Append($"<{Name.ToUpperInvariant()}>");
            foreach (var c in Children)
            {
                result.Append(c.ToString());
            }
            result.Append($"</{Name.ToUpperInvariant()}>");
            return result.ToString();
        }
        else
        {
            return $"<{Name.ToUpperInvariant()}>{Value}";
        }
    }

    /// <summary>
    /// Convert this instance to strings.
    /// </summary>
    /// <returns>Enumeration of string reprenting the property.</returns>
    /// <remarks>
    /// This method is provided to allow serializers to customize their output streams.
    /// </remarks>
    public IEnumerable<String> ToStrings()
    {
        if (Children.Count > 0)
        {
            yield return $"<{Name}>";
            foreach (var c in Children)
            {
                yield return string.Concat(c.ToStrings());
            }
            yield return $"</{Name}>";
        }
        else
        {
            yield return $"<{Name}>{Value}";
        }
    }
}
