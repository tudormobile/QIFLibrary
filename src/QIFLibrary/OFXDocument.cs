using System.Text;
using Tudormobile.QIFLibrary.IO;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanism for reading and writing data in Open Financial Exchange Format (OFX).
/// </summary>
public class OFXDocument
{
    private const string DEFAULT_VERSION = "102";
    private const string VERSION_KEY = "VERSION";
    private string? _version;

    /// <summary>
    /// HTTP COntent Type
    /// </summary>
    public static readonly string CONTENT_TYPE = "application/x-ofx";

    /// <summary>
    /// Default file extension.
    /// </summary>
    public static readonly string FILE_EXTENSION = "ofx";

    /// <summary>
    /// OFX Document Version.
    /// </summary>
    public string Version
    {
        get => _version ?? (Headers[VERSION_KEY].Length > 0 ? Headers[VERSION_KEY] : DEFAULT_VERSION);
        set => _version = value;
    }

    /// <summary>
    /// Document headers.
    /// </summary>
    public OFXHeaders Headers { get; } = new OFXHeaders();

    /// <summary>
    /// Ordered list of message sets in the document.
    /// </summary>
    public IList<OFXMessageSet> MessageSets { get; } = [];

    /// <summary>
    /// Root element, if it exists.
    /// </summary>
    public OFXProperty Root { get; private set; } = new OFXProperty("");

    /// <summary>
    /// Adds default headers, overwriting any existing of the same name.
    /// </summary>
    /// <returns>Fluent-reference to this document.</returns>
    public OFXDocument DefaultHeaders()
    {
        foreach (var x in OFXHeaders.Default(version: this.Version).AsEnumerable())
        {
            Headers[x.Key] = x.Value;
        }
        return this;
    }

    /// <summary>
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="utf8Stream">OFX text to parse.</param>
    /// <param name="leaveOpen">Leave the provided stream open [OPTONAL; Default = true].</param>
    /// <returns>An OFXDocument representation of the data.</returns>
    public static OFXDocument Parse(Stream utf8Stream, bool leaveOpen = true) => Parse(new StreamReader(utf8Stream, Encoding.UTF8), leaveOpen);

    /// <summary>
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="ofxData">OFX text to parse.</param>
    /// <returns>A OFXDocument representation of the data.</returns>
    public static OFXDocument Parse(string ofxData) => Parse(new StringReader(ofxData));

    /// <summary>
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="path">The path to the file to be parsed.</param>
    /// <returns>An OFXDocument representation of the data.</returns>
    public static OFXDocument ParseFile(string path) => Parse(File.OpenText(path));

    /// <summary>
    /// Save data to a file.
    /// </summary>
    /// <param name="path">Pathname to the file.</param>
    public void Save(string path)
    {
        using var s = new StreamWriter(path);
        new OFXWriter(s).Write(this, indent: true);
    }

    private static OFXDocument Parse(TextReader reader, bool leaveOpen = false)
    {
        try
        {
            var result = new OFXDocument();
            var ofxReader = new OFXReader(reader);

            // Read the headers
            while (ofxReader.TryReadHeader(out var header))
            {
                result.Headers[header.Key] = header.Value;
            }

            // Peek all data to see if we missed any headers from malformed OFX files such
            // as those produced by Chase Bank. (Headers begins with a blank line and does 
            // NOT contain the required end line).
            if (result.Headers["OFXHEADER"] == string.Empty || result.Headers.Count == 0)
            {
                if (ofxReader.TryForceReadHeaders(out var headers))
                {
                    foreach (var (Key, Value) in headers!)
                    {
                        result.Headers.Add(Key, Value);
                    }
                }
            }

            // Move to the start of the OFX data
            if (ofxReader.TryMoveToStart(out OFXTokenReader.OFXToken? token, "OFX"))
            {
                result.Root = new OFXProperty("OFX");
                // Parse into MessageSets
                while (ofxReader.TryReadMessageSet(out var messageSet))
                {
                    result.MessageSets.Add(messageSet);
                    var messageSetProp = new OFXProperty(messageSet.MessageSetType.ToString());
                    foreach (var m in messageSet.Messages)
                    {
                        var messageProp = new OFXProperty(m.Name);
                        foreach (var child in m.Properties)
                        {
                            messageProp.Children.Add(child);
                        }
                        messageSetProp.Children.Add(messageProp);
                    }
                    result.Root.Children.Add(messageSetProp);
                }
            }
            return result;
        }
        finally
        {
            if (!leaveOpen)
            {
                reader.Close();
            }
        }
    }

}
