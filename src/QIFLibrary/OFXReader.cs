namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanisms to read OFX data.
/// </summary>
public class OFXReader
{
    private readonly bool _ignoreWhiteSpace;
    private readonly TextReader _reader;
    private readonly Lazy<OFXTokenReader> _tokenReader;

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="reader">Text reader to use as the data source.</param>
    /// <param name="ignoreWhiteSpace">True if whitespace should be ignored.</param>
    public OFXReader(TextReader reader, bool ignoreWhiteSpace = true)
    {
        _reader = reader;
        _ignoreWhiteSpace = ignoreWhiteSpace;
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

    /// <summary>
    /// Attempts for force-read headers from malformed OFX data.
    /// </summary>
    /// <param name="headers">Collection of Key/Value pair if successful.</param>
    /// <returns>True if successful; otherwise false.</returns>
    /// <remarks>
    /// This method is intended for malformed OFX data. It does not
    /// always consumes at least 1 line of content. It will attempt to
    /// parse headers until an open xml tag is found. Internally, this
    /// method reads OFX tokens from the parser rather than 'lines' per
    /// the OFX specification.
    /// </remarks>
    public bool TryForceReadHeaders(out (string Key, string Value)[]? headers)
    {
        var ignore = _tokenReader.Value.IgnoreWhiteSpace;
        _tokenReader.Value.IgnoreWhiteSpace = true;
        var foundHeaders = new List<(string Key, string Value)>();

        var line = _tokenReader.Value.Peek();
        while (line.TokenType == OFXTokenReader.OFXTokenType.Content)
        {
            foreach (var item in line.Data.Split('\n'))
            {
                var parts = item.Split(':');

                if (parts.Length == 2 && parts[0].Trim().Length > 0)
                {
                    foundHeaders.Add((parts[0].Trim(), parts[1].Trim()));
                }
            }
            _ = _tokenReader.Value.Read();      // discard the above
            line = _tokenReader.Value.Peek();   // and peek the next one.
        }
        if (foundHeaders.Count > 0)
        {
            headers = foundHeaders.ToArray();
            return true;
        }
        headers = default;
        return false;
    }

    /// <summary>
    /// Read the next message set.
    /// </summary>
    /// <param name="messageSet">Message set if successful.</param>
    /// <returns>True if successful; otherwise false.</returns>
    public bool TryReadMessageSet(out OFXMessageSet messageSet)
    {
        var result = false;
        messageSet = new OFXMessageSet(OFXMessageSetTypes.UNKNOWN, OFXMessageDirection.UNKNOWN, version: 0);
        if (TryMoveToStart(out var startToken))
        {
            var name = startToken!.Data;
            messageSet = setFromName(name);

            while (TryReadMessage(out var message))
            {
                messageSet.Messages.Add(message!);
            }
            // for now only
            TryMoveToEnd(out var endToken, startToken.Data);
            // end for now only
            result = true;
        }
        return result;
    }

    /// <summary>
    /// Read the next message.
    /// </summary>
    /// <param name="message">Message read or (null) if not found.</param>
    /// <returns>True if message was found; otherwise false.</returns>
    public bool TryReadMessage(out OFXMessage? message)
    {
        var result = false;
        message = default;
        // Should start with a start tag
        _tokenReader.Value.IgnoreWhiteSpace = true;
        var peek = _tokenReader.Value.Peek();
        if (peek.TokenType == OFXTokenReader.OFXTokenType.StartTag)
        {
            var startTag = _tokenReader.Value.Read();
            message = new OFXMessage() { Name = startTag.Data };
            // Read the message until the matching end tag (or EOF)
            while (!peekEOF() && !peekEndTag(startTag.Data))
            {
                var tag = _tokenReader.Value.Read();
                // collect properties
                if (tag.TokenType == OFXTokenReader.OFXTokenType.StartTag)
                {
                    if (tryReadProperty(startTag.Data, tag.Data, out OFXProperty? prop))
                    {
                        // check for well-known properties
                        if (prop!.Name == "STATUS")
                        {
                            message.Status = new OFXStatus(
                                int.TryParse(prop.Children.FirstOrDefault(c => c.Name == "CODE")?.Value ?? String.Empty, out var code) ? code : 0,
                                Enum.TryParse<OFXStatus.StatusSeverity>(prop.Children.FirstOrDefault(c => c.Name == "SEVERITY")?.Value ?? String.Empty, out var severity) ? severity : OFXStatus.StatusSeverity.UNKNOWN,
                                prop.Children.FirstOrDefault(c => c.Name == "MESSAGE")?.Value ?? String.Empty
                            );
                        }
                        else if (prop.Name == "TRNUID")
                        {
                            message.Id = prop.Value;
                        }
                        else
                        {
                            message.Properties.Add(prop);
                        }
                    }
                }
            }
            var endTag = _tokenReader.Value.Read();
            if (endTag.TokenType == OFXTokenReader.OFXTokenType.EndTag && endTag.Data == startTag.Data)
            {
                result = true;
            }
        }
        return result;
    }

    /// <summary>
    /// Move position forward to a start tag.
    /// </summary>
    /// <param name="name">Name of the start token to seek.</param>
    /// <param name="token">If success, the start token; otherwise (null).</param>
    /// <returns>True if start token found, otherwise false.</returns>
    public bool TryMoveToStart(out OFXTokenReader.OFXToken? token, string? name = null)
    {
        do
        {
            token = _tokenReader.Value.Read();
        } while (token.TokenType != OFXTokenReader.OFXTokenType.EndOfFile && !isMatch(token, OFXTokenReader.OFXTokenType.StartTag, name));
        return isMatch(token, OFXTokenReader.OFXTokenType.StartTag, name);
    }

    /// <summary>
    /// Move position forward to a end tag.
    /// </summary>
    /// <param name="name">Name of the end token to seek.</param>
    /// <param name="token">If success, the end token; otherwise (null).</param>
    /// <returns>True if end token found, otherwise false.</returns>
    public bool TryMoveToEnd(out OFXTokenReader.OFXToken? token, string? name = null)
    {
        do
        {
            token = _tokenReader.Value.Read();
        } while (token.TokenType != OFXTokenReader.OFXTokenType.EndOfFile && !isMatch(token, OFXTokenReader.OFXTokenType.EndTag, name));
        return isMatch(token, OFXTokenReader.OFXTokenType.EndTag, name);
    }

    private bool tryReadProperty(string outerTag, string innerTag, out OFXProperty? prop)
    {
        bool result = false;
        prop = new OFXProperty(innerTag);
        while (!peekEOF() && !peekEndTag(outerTag))
        {
            // consume stuff
            var tag = _tokenReader.Value.Read();
            if (tag.TokenType == OFXTokenReader.OFXTokenType.Content)
            {
                prop.Value = tag.Data;
                result = true; break;
            }
            else if (tag.TokenType == OFXTokenReader.OFXTokenType.EndTag && tag.Data == innerTag)
            {
                result = true; break;
            }
            else if (tag.TokenType == OFXTokenReader.OFXTokenType.StartTag)
            {
                if (tryReadProperty(outerTag, tag.Data, out var p))
                {
                    prop.Children.Add(p!);
                    result = true;
                }
            }
        }

        return result;
    }

    private bool peekEOF()
        => _tokenReader.Value.Peek().TokenType == OFXTokenReader.OFXTokenType.EndOfFile;

    private bool peekEndTag(string name)
        => _tokenReader.Value.Peek().TokenType == OFXTokenReader.OFXTokenType.EndTag &&
           _tokenReader.Value.Peek().Data == name;

    private bool isMatch(OFXTokenReader.OFXToken token, OFXTokenReader.OFXTokenType tokenType, string? name = null)
        => token.TokenType == tokenType && (string.IsNullOrEmpty(name) || token.Data == name);

    private OFXMessageSet setFromName(string name)
    {
        var index = name.IndexOf("MSGSR");
        int v = 0;
        var valid = index > 0 && name.Length == index + 8 && int.TryParse(name.Substring(name.Length - 1), out v);
        if (valid)
        {
            var t = Enum.TryParse(typeof(OFXMessageSetTypes), name.Substring(0, index), out var foundType) ? (OFXMessageSetTypes)foundType : OFXMessageSetTypes.UNKNOWN;
            var d = name[name.Length - 3] switch
            {
                'Q' => OFXMessageDirection.REQUEST,
                'S' => OFXMessageDirection.RESPONSE,
                _ => OFXMessageDirection.UNKNOWN,
            };
            return new OFXMessageSet(t, d, v);
        }
        return new OFXMessageSet(OFXMessageSetTypes.UNKNOWN, OFXMessageDirection.UNKNOWN, version: 0);
    }

}
