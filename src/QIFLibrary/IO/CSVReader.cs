using System.Text;

namespace Tudormobile.QIFLibrary.IO;

/// <summary>
/// Provides mechanisms for reading CSV data.
/// </summary>
public class CSVReader
{
    private string[]? _fields = null;
    private string? _lastLine = null;
    private readonly bool? _hasHeader;
    private readonly TextReader _reader;

    /// <summary>
    /// Create and initialize a new CSV data reader.
    /// </summary>
    /// <param name="stream">Stream to read.</param>
    /// <param name="hasHeader"></param>
    /// <returns>A new CSV reader.</returns>
    /// <remarks>
    /// Call Close() when finished with the reader, which will also close the provided stream.
    /// </remarks>
    public static CSVReader FromStream(Stream stream, bool? hasHeader = null)
        => new(new StreamReader(stream), hasHeader);

    /// <summary>
    /// Create and initialize a new instance.
    /// </summary>
    /// <param name="reader">Text reader to use as CSV data source.</param>
    /// <param name="hasHeader">Indicates if the CSV data has a header or not.</param>
    public CSVReader(TextReader reader, bool? hasHeader = null)
    {
        _hasHeader = hasHeader;
        _reader = reader;
    }

    /// <summary>
    /// Read comments from the data stream.
    /// </summary>
    /// <returns>Comments</returns>
    public IEnumerable<string> ReadComments()
    {
        _lastLine = _reader.ReadLine();
        while (_lastLine != null && !_lastLine.Contains(','))
        {
            yield return _lastLine;
            _lastLine = _reader.ReadLine();
        }
    }

    /// <summary>
    /// Read header from the data stream.
    /// </summary>
    /// <returns>Header field list.</returns>
    public IEnumerable<string> ReadHeader()
    {
        var header = _lastLine ?? _reader.ReadLine();
        _lastLine = null;
        _fields = header?.CsvSplit();
        return _fields ?? Enumerable.Empty<string>();
    }

    /// <summary>
    /// Reads a record from the data stream.
    /// </summary>
    /// <returns>A new csv record if available; otherwise (null).</returns>
    public CSVDocument.ICSVRecord? ReadRecord()
    {
        if (_fields == null && _hasHeader == true) ReadHeader();
        var line = _lastLine ?? _reader.ReadLine();
        _lastLine = null;
        return line == null ? null : new CSVRecord(line, _fields);
    }

    /// <summary>
    /// Reads all of the records from the stream.
    /// </summary>
    /// <returns>Collection of CSV records.</returns>
    public IEnumerable<CSVDocument.ICSVRecord> ReadRecords()
    {
        CSVDocument.ICSVRecord? record = ReadRecord();
        while (record != null)
        {
            yield return record;
            record = ReadRecord();
        }
    }

    /// <summary>
    /// Reads a record asychronously from the stream.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public async Task<CSVDocument.ICSVRecord?> ReadRecordAsync()
        => await Task.FromResult(ReadRecord());

    /// <summary>
    /// Reads all records asynchronously from the stream.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public async IAsyncEnumerable<CSVDocument.ICSVRecord> ReadRecordsAsync()
    {
        CSVDocument.ICSVRecord? record;
        while ((record = await ReadRecordAsync()) != null)
        {
            yield return record;
        }
    }

    private class CSVRecord : CSVDocument.ICSVRecord
    {
        private readonly string _line;
        private readonly string[]? _fields;
        private string[]? _values;

        public CSVRecord(string line, string[]? fields)
        {
            _line = line;
            _fields = fields;
        }

        string CSVDocument.ICSVRecord.this[int index]
            => ((CSVDocument.ICSVRecord)this).Values[index];

        string CSVDocument.ICSVRecord.this[string fieldName]
            => ((CSVDocument.ICSVRecord)this)[Array.IndexOf(_fields!, fieldName)];

        string[] CSVDocument.ICSVRecord.Values
            => _values ?? (_line.Contains('\"') ? parserSplit() : simpleSplit());

        bool CSVDocument.ICSVRecord.TryGetValue<T>(int index, out T value)
            => T.TryParse(((CSVDocument.ICSVRecord)this)[index], null, out value!);

        bool CSVDocument.ICSVRecord.TryGetValue<T>(string fieldName, out T value)
            => T.TryParse(((CSVDocument.ICSVRecord)this)[fieldName], null, out value!);

        private string[] simpleSplit()
        {
            _values = _line.Split(',');
            return _values;
        }

        private string[] parserSplit()
        {
            _values = parseLine().ToArray();
            return _values;
        }

        private IEnumerable<string> parseLine()
        {
            StringBuilder part = new StringBuilder();
            bool inQuote = false;
            for (int i = 0; i < _line.Length; i++)
            {
                if (!inQuote)
                {
                    if (_line[i] == ',')
                    {
                        yield return part.ToString();
                        part.Clear();
                    }
                    else if (_line[i] == '"')
                    {
                        inQuote = true;
                        part.Append(_line[i]);
                    }
                    else
                    {
                        part.Append(_line[i]);
                    }
                }
                else
                {
                    if (_line[i] == '"')
                    {
                        inQuote = false;
                    }
                    part.Append(_line[i]);
                }
            }
            yield return part.ToString();
        }

    }
}

internal static class CSVExtentions
{
    private static Char[] _trimChars = [' ', '\r', '\n'];
    public static string[] CsvSplit(this string s)
    {
        return csvSplitString(s).Select(x => x.Trim(_trimChars)).ToArray();
    }

    public static string Quote(this string s)
    {
        if (s.Length < 2 || s[0] != '\"' || s[s.Length - 1] != '\"') return $"\"{s}\"";
        return s;
    }

    public static string UnQuote(this string s)
    {
        if (s.Length >= 2 && s[0] == '\"' && s[s.Length - 1] == '\"') return s.Substring(1, s.Length - 2);
        return s;
    }

    private static IEnumerable<string> csvSplitString(string s)
    {
        var start = 0;
        var inQuote = false;
        for (int i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (!inQuote)
            {
                if (c == '"')
                {
                    inQuote = true;
                }
                else if (c == ',')
                {
                    yield return s.Substring(start, i - start).UnQuote();
                    start = i + 1;
                }
            }
            else
            {
                if (c == '"') inQuote = false;
            }
        }
        yield return s.Substring(start, s.Length - start).UnQuote();
    }
}
