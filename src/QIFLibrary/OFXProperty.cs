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
    public List<OFXProperty> Children { get; } = new List<OFXProperty>();

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
}
