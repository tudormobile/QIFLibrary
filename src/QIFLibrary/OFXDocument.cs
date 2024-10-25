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
    /// <summary>
    /// HTTP COntent Type
    /// </summary>
    public static readonly string CONTENT_TYPE = "application/x-ofx";

    /// <summary>
    /// Default file extension.
    /// </summary>
    public static readonly string FILE_EXTENSION = "ofx";

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

            while (ofxReader.TryReadHeader(out var header))
            {
                result.Headers[header.Key] = header.Value;
            }

            var settings = new XmlReaderSettings()
            {
                IgnoreWhitespace = true,
            };
            var xmlReader = XmlReader.Create(reader, settings);
            try
            {
                xmlReader.Read();
                if (xmlReader.IsStartElement("OFX"))
                {
                    // Read all the message sets.
                    while (xmlReader.Read())
                    {
                        Debug.WriteLine(xmlReader.Name);
                        var s = xmlReader.ReadOuterXml();
                    }
                }
            }
            finally
            {
                xmlReader.Close();
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
