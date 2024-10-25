using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechnism for reading OFX data into tokens.
/// </summary>
/// <remarks>
/// Limitation: tokens cannot exceed 4k.
/// </remarks>
public class OFXTokenReader
{
    private TextReader _reader;
    private byte[] _buffer = new byte[4096];
    private int _position = 0;
    private static char[] _trimChars = { '<', '/', '>', ' ' };
    private OFXToken? _peek;

    /// <summary>
    /// Create and initialize a new instance.
    /// </summary>
    /// <param name="reader"></param>
    public OFXTokenReader(TextReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// Previous the next token without consuming it.
    /// </summary>
    /// <returns>A preview of the next token.</returns>
    public OFXToken Peek()
    {
        _peek = _peek ?? Read();
        return _peek;
    }

    /// <summary>
    /// Reads the next token.
    /// </summary>
    /// <returns>The next token.</returns>
    public OFXToken Read()
    {
        var result = _peek ?? readImpl();
        _peek = null;
        return result;
    }

    /// <summary>
    /// Reads a token asynchronously.
    /// </summary>
    /// <returns>A Task that represents the read operation.</returns>
    public Task<OFXToken> ReadAsync() => Task.FromResult(Read());

    /// <summary>
    /// Reads tokens asychronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public async IAsyncEnumerable<OFXToken> ReadTokensAsync()
    {
        OFXToken? token;
        do
        {
            token = await ReadAsync();
            yield return token;
        } while (token.TokenType != OFXTokenType.EndOfFile);
    }

    /// <summary>
    /// A 'token' in an OFX file.
    /// </summary>
    public class OFXToken(OFXTokenType tokenType, string data)
    {
        /// <summary>
        /// Type of token.
        /// </summary>
        public OFXTokenType TokenType => tokenType;

        /// <summary>
        /// Token data.
        /// </summary>
        public String Data => data;
    }

    /// <summary>
    /// Type of OFX tokens.
    /// </summary>
    public enum OFXTokenType
    {
        /// <summary>
        /// Reader is at the end of file.
        /// </summary>
        EndOfFile,

        /// <summary>
        /// Start of an OFX tag.
        /// </summary>
        /// <remarks>This tag name, such s 'OFX', is contained in the data.</remarks>
        StartTag,

        /// <summary>
        /// End of an OFX tag.
        /// </summary>
        /// <remarks>This tag name, such s 'OFX', is contained in the data.</remarks>
        EndTag,

        /// <summary>
        /// Start of an OFX tag.
        /// </summary>
        /// <remarks>This whitespace is contained in the data.</remarks>
        Whitespace,

        /// <summary>
        /// Content.
        /// </summary>
        /// <remarks>This content value is contained in the data.</remarks>
        Content,
    }

    private OFXToken readImpl()
    {
        _position = 0;

        // accumulate token data
        bool accumulating = true;
        do
        {
            var c = _reader.Peek();

            if (c == '<' && _position > 0)
            {
                accumulating = false;
            }
            else if (c == '>')
            {
                _buffer[_position++] = (byte)_reader.Read();
                accumulating = false;
            }
            else if (c == -1)
            {
                accumulating = false;
            }
            else
            {
                _buffer[_position++] = (byte)_reader.Read();
            }
        } while (accumulating);

        // process token data

        // End of file?
        if (_position == 0) return new OFXToken(OFXTokenType.EndOfFile, String.Empty);

        // Whitespace?
        string tag = Encoding.UTF8.GetString(_buffer, 0, _position);
        if (String.IsNullOrWhiteSpace(tag)) return new OFXToken(OFXTokenType.Whitespace, tag);

        // start or end element?
        if (_position > 2 && tag[_position - 1] == '>') return tag[1] == '/' ?
                new OFXToken(OFXTokenType.EndTag, tag.Trim(_trimChars)) :
                new OFXToken(OFXTokenType.StartTag, tag.Trim(_trimChars));

        // Must be content
        return new OFXToken(OFXTokenType.Content, tag.TrimEnd());
    }

}
