

using System.Runtime.CompilerServices;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanisms to read OFX data.
/// </summary>
public class OFXReader
{
    private bool _ignoreWhitespace;
    private TextReader _reader;
    private readonly Lazy<OFXTokenReader> _tokenReader;

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="reader">Text reader to use as the data source.</param>
    /// <param name="ignoreWhitespace">True if whitespace should be ignored.</param>
    public OFXReader(TextReader reader, bool ignoreWhitespace = true)
    {
        _reader = reader;
        _ignoreWhitespace = ignoreWhitespace;
        _tokenReader = new Lazy<OFXTokenReader>(() => new OFXTokenReader(_reader));
    }

    /// <summary>
    /// Attempts to read a header from the stream.
    /// </summary>
    /// <param name="header">Key/Value pair if successful.</param>
    /// <returns>True if successful; otherwise false.</returns>
    /// <remarks>
    /// This method always consumes at least 1 line of content from the stream. A FormatException is thrown if non-header is encountered.
    /// </remarks>
    public bool TryReadHeader(out (string Key, string Value) header)
    {
        var line = _reader.ReadLine();
        if (!String.IsNullOrWhiteSpace(line))
        {
            var parts = line.Split(':');

            if (parts.Length == 2)
            {
                header.Key = parts[0].Trim();
                header.Value = parts[1].Trim();
                return header.Key.Length > 0;
            }
        }
        header = default;
        return false;
    }
}