using System;
using System.Reflection.PortableExecutable;
using Tudormobile.QIFLibrary;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanisms to write OFX data.
/// </summary>
public class OFXWriter
{
    private bool _indent;
    private readonly TextWriter _writer;
    private readonly int INDENT_SIZE = 2;

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="writer">Text writer to use as the data sink.</param>
    /// <param name="indent">True if indents and newlines (pretty output).</param>
    public OFXWriter(TextWriter writer, bool indent = true)
    {
        _writer = writer;
        _indent = indent;
    }

    /// <summary>
    /// Writes a document.
    /// </summary>
    /// <param name="document">Document to write.</param>
    /// <param name="indent">True if document is formatted and indented (OPTIONAL; default = true)</param>
    public void Write(OFXDocument document, bool? indent = null)
    {
        _indent = indent ?? _indent;
        _writer.Write(document.Headers.ToString());
        _writer.Write("<OFX>");
        var lastOpen = true;
        var indentAmount = _indent ? INDENT_SIZE : 0;
        var currentIndent = 0;

        // Order of message sets determined by type (required)
        var indentString = "";
        foreach (var set in document.MessageSets.OrderBy(x => x.MessageSetType))
        {
            // indentation determine by open tag
            foreach (var s in set.ToStrings())
            {
                var isOpen = s[1] != '/';
                if (lastOpen && isOpen)
                {
                    currentIndent += indentAmount;
                    indentString = $"{Environment.NewLine}{new String(' ', currentIndent)}";
                }
                if (!isOpen)
                {
                    currentIndent -= indentAmount;
                    indentString = $"{Environment.NewLine}{new String(' ', currentIndent)}";
                }
                lastOpen = isOpen && s[^1] == '>';
                if (currentIndent > 0) _writer.Write(indentString);
                _writer.Write(s);
            }
        }

        _writer.Write(_indent ? $"{Environment.NewLine}</OFX>" : "</OFX>");
    }

}