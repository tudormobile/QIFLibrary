using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="utf8Stream">OFX text to parse.</param>
    /// <param name="leaveOpen">Leave the provided stream open [OPTONAL; Default = true].</param>
    /// <returns>An OFXDocument representation of the data.</returns>
    public static OFXDocument Parse(Stream utf8Stream, bool leaveOpen = true) => parse(new StreamReader(utf8Stream, Encoding.UTF8), leaveOpen);

    /// <summary>
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="ofxData">OFX text to parse.</param>
    /// <returns>A OFXDocument representation of the data.</returns>
    public static OFXDocument Parse(string ofxData) => parse(new StringReader(ofxData));

    /// <summary>
    /// Parses data into a OFX Document.
    /// </summary>
    /// <param name="path">The path to the file to be parsed.</param>
    /// <returns>An OFXDocument representation of the data.</returns>
    public static OFXDocument ParseFile(string path) => parse(File.OpenText(path));

    private static OFXDocument parse(TextReader reader, bool leaveOpen = false)
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

            // Move to the start of the OFX data
            if (ofxReader.TryMoveToStart(out OFXTokenReader.OFXToken? token, "OFX"))
            {
                // Parse into MessageSets
                while (ofxReader.TryReadMessageSet(out var messageSet))
                {
                    result.MessageSets.Add(messageSet);
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
