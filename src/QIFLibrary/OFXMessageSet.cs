using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Message Set.
/// </summary>
public class OFXMessageSet
{
    /// <summary>
    /// Message set version number.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Message set type.
    /// </summary>
    public OFXMessageSetTypes MessageSetType { get; }

    /// <summary>
    /// Direction (Request or Response).
    /// </summary>
    public OFXMessageDirection Direction { get; }

    /// <summary>
    /// Ordered list of message sets in the document.
    /// </summary>
    public IList<OFXMessage> Messages { get; } = [];

    /// <summary>
    /// Create and initialize a new message set.
    /// </summary>
    /// <param name="messageSetType">Message set type.</param>
    /// <param name="direction">Message set direction.</param>
    /// <param name="version">Optional: Message set version (default=1).</param>
    public OFXMessageSet(OFXMessageSetTypes messageSetType, OFXMessageDirection direction, int version = 1)
    {
        Version = version;
        MessageSetType = messageSetType;
        Direction = direction;
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
        yield return $"<{MessageSetType}MSGSETV{Version}>";

        // Messages
        foreach (var m in Messages)
        {
            foreach (var s in m.ToStrings()) yield return s;
        }

        yield return $"</{MessageSetType}MSGSETV{Version}>";
    }
}

/// <summary>
/// OFX Message Set and Message direction, request or response.
/// </summary>
public enum OFXMessageDirection
{
    /// <summary>
    /// Unknown message direction.
    /// </summary>
    UNKNOWN = 0,
    /// <summary>
    /// Message Request.
    /// </summary>
    REQUEST = 1,
    /// <summary>
    /// Message Response.
    /// </summary>
    RESPONSE = 2
}

/// <summary>
/// OFX Message Set Types.
/// </summary>
public enum OFXMessageSetTypes
{
    /*
Signon	<SIGNONMSGSETV1>	1
Signup	<SIGNUPMSGSETV1>	1
Banking	<BANKMSGSETV1>	1
Credit Card Statements	<CREDITCARDMSGSETV1>	1
Investment Statements	<INVSTMTMSGSETV1>	1
Interbank Funds Transfers	<INTERXFERMSGSETV1>	1
Wire Funds Transfers	<WIREXFERMSGSETV1>	1
Payments	<BILLPAYMSGSETV1>	1
General e-mail	<EMAILMSGSETV1>	1
Investment security list	<SECLISTMSGSETV1>	1
FI Profile	<PROFMSGSETV1>	1
     */
    /// <summary>
    /// Unknown message set.
    /// </summary>
    UNKNOWN = 0,
    /// <summary>
    /// Signon.
    /// </summary>
    SIGNON,
    /// <summary>
    /// Signup
    /// </summary>
    SIGNUP,
    /// <summary>
    /// Banking.
    /// </summary>
    BANK,
    /// <summary>
    /// Credit Card Statements.
    /// </summary>
    CREDITCARD,
    /// <summary>
    /// Investment Statements.
    /// </summary>
    INVSTMT,
    /// <summary>
    /// Interbank Funds Transfers.
    /// </summary>
    INTERXFER,
    /// <summary>
    /// Wire Funds Transfers.
    /// </summary>
    WIREXFER,
    /// <summary>
    /// Payments.
    /// </summary>
    BILLPAY,
    /// <summary>
    /// General e-mail.
    /// </summary>
    EMAIL,
    /// <summary>
    /// Security List.
    /// </summary>
    SECLIST,
    /// <summary>
    /// FI Profile.
    /// </summary>
    PROF,
}
