namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Message Status.
/// </summary>
public class OFXStatus
{
    /// <summary>
    /// Status code.
    /// </summary>
    public int Code { get; set; } = 0;

    /// <summary>
    /// Status message.
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// Status severity.
    /// </summary>
    public StatusSeverity Severity { get; set; } = StatusSeverity.UNKNOWN;

    /// <summary>
    /// Status message severity values.
    /// </summary>
    public enum StatusSeverity
    {
        /// <summary>
        /// Unknown severity (invalid value).
        /// </summary>
        UNKNOWN,

        /// <summary>
        /// Information.
        /// </summary>
        INFO,

        /// <summary>
        /// Warning.
        /// </summary>
        WARN,

        /// <summary>
        /// Error.
        /// </summary>
        ERROR
    }
}

