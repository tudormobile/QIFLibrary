using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;

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
        yield return "<STATUS>";
        yield return $"<CODE>{Code}";
        yield return $"<SEVERITY>{Severity}";
        if (Message.Length > 0)
        {
            yield return $"<MESSAGE>{Message}";    // Message is optional.
        }
        yield return "</STATUS>";

    }
}

