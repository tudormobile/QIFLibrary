using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Data Headers
/// </summary>
public class OFXHeaders
{
    private Dictionary<string, string> _headers = [];

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">Header key.</param>
    /// <returns>Header value for the key</returns>
    public string this[string key]
    {
        get => HasValue(key) ? _headers[key] : String.Empty;
        set => _headers[key] = value;
    }

    /// <summary>
    /// Query if a given header is present.
    /// </summary>
    /// <param name="key">Name (key) of the header to query.</param>
    /// <returns>(True) if the header us set; otherwise (false).</returns>
    public bool HasValue(string key) => _headers.ContainsKey(key);

    /// <summary>
    /// Gets the number of headers.
    /// </summary>
    public int Count => _headers.Count;

    /// <summary>
    /// Provides an enumeration of the headers.
    /// </summary>
    /// <returns>An enumeration of key-value pairs.</returns>
    public IEnumerable<KeyValuePair<string, string>> AsEnumerable() => _headers;

    /// <summary>
    /// Creates a new instance of default headers.
    /// </summary>
    /// <returns>Default headers.</returns>
    /// <remarks>Default() uses version 1.02 of OFX specification.</remarks>
    public static OFXHeaders Default() => new OFXHeaders()
        .Add("OFXHEADER", "100")
        .Add("DATA", "OFXSGML")
        .Add("VERSION", "102")
        .Add("SECURITY", "NONE")
        .Add("ENCODING", "USASCII")
        .Add("CHARSET", "1252")
        .Add("COMPRESSION", "NONE")
        .Add("OLDFILEUID", "NONE")
        .Add("NEWFILEUID", "NONE");

    /// <summary>
    /// Add a new header value.
    /// </summary>
    /// <param name="key">Header key.</param>
    /// <param name="value">Header value.</param>
    /// <returns>Fluent-reference to self.</returns>
    public OFXHeaders Add(string key, string value)
    {
        this[key] = value;
        return this;
    }

}
