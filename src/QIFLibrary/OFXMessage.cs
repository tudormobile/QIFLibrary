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
    /// Message status.
    /// </summary>
    public OFXStatus Status { get; set; } = new OFXStatus();
}

