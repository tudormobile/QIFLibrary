using Tudormobile.QIFLibrary.IO;
namespace Tudormobile.QIFLibrary;

/// <summary>
/// Comma Separated Values (CSV) document.
/// </summary>
public class CSVDocument
{
    private static char[] _lineEndings = ['\r', '\n'];
    private int _recordStart = 0;
    private string[]? _data = null;
    private readonly Lazy<IList<String>> _comments;

    /// <summary>
    /// HTTP Content Type
    /// </summary>
    public static readonly string CONTENT_TYPE = "text/csv";

    /// <summary>
    /// Default file extension.
    /// </summary>
    public static readonly string FILE_EXTENSION = "csv";

    /// <summary>
    /// Document name (optional).
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// Document comments.
    /// </summary>
    public IList<String> Comments => _comments.Value;

    /// <summary>
    /// List of records.
    /// </summary>
    public IList<ICSVRecord> Records { get; private set; } = [];

    /// <summary>
    /// List of fields.
    /// </summary>
    public IList<String> Fields { get; } = [];

    /// <summary>
    /// Creates a new CSV document.
    /// </summary>
    /// <param name="name">Name of the document (optional).</param>
    /// <param name="comments">Comments (optional).</param>
    public CSVDocument(string? name = null, IEnumerable<string>? comments = null)
    {
        Name = name ?? string.Empty;
        _comments = comments == null ? new Lazy<IList<string>>(() => readComments()) : new Lazy<IList<string>>(new List<String>(comments));
    }

    /// <summary>
    /// Load and parse a file.
    /// </summary>
    /// <param name="path">Pathname of the file.</param>
    /// <returns>A CSVDocument representing the file.</returns>
    public static CSVDocument ParseFile(string path)
        => Parse(File.ReadAllLines(path), Path.GetFileNameWithoutExtension(path));

    /// <summary>
    /// Parse CSV data.
    /// </summary>
    /// <param name="data">CSV data to parse.</param>
    /// <param name="name">Name of the document.</param>
    /// <returns>A CSVDocument representing the data.</returns>
    public static CSVDocument Parse(string data, string? name = null)
        => Parse(data.Split(_lineEndings, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries), name);

    /// <summary>
    /// Parse CSV data.
    /// </summary>
    /// <param name="csvData">Lines of CSV data.</param>
    /// <param name="name">Name of the document (OPTIONAL)</param>
    /// <returns>A CSVDocument representing the data.</returns>
    public static CSVDocument Parse(string[] csvData, string? name = null)
    {
        var result = new CSVDocument(name) { _data = csvData };

        // Try and determine records and fields.
        result._recordStart = result.Comments.Count;
        if (result._data.Length - result._recordStart > 1)
        {
            // at least 2 records
            var first = result._data[result._recordStart];
            var second = result._data[result._recordStart + 1];
            var last = result._data[^1];

            if (!typesMatch(getTypes(first), getTypes(last)))
            {
                // assume header
                ((List<String>)result.Fields).AddRange(first.CsvSplit());
                result._recordStart++;
            }
        }
        result.Records = new List<ICSVRecord>(Enumerable.Range(result._recordStart, result._data.Length - result._recordStart).Select(x => new CSVRecord(x, result)));
        return result;
    }

    /// <summary>
    /// Represents a CSV record
    /// </summary>
    public interface ICSVRecord
    {
        /// <summary>
        /// Access data via index.
        /// </summary>
        /// <param name="index">Zero based index to the field of interest.</param>
        /// <returns>Record data of type 'String'.</returns>
        public string this[int index] { get; }

        /// <summary>
        /// Access data via field name.
        /// </summary>
        /// <param name="fieldName">Name of the field to access.</param>
        /// <returns>Record data of type 'String'.</returns>
        public string this[string fieldName] { get; }

        /// <summary>
        /// Access data via index.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="index">Zero based index to the field of interest.</param>
        /// <param name="value">Record data of type 'T'.</param>
        /// <returns>(True) if successful, otherwise (False).</returns>
        public bool TryGetValue<T>(int index, out T? value) where T : IParsable<T>;

        /// <summary>
        /// Access data via field name.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">Record data of type 'T'.</param>
        /// <returns>(True) if successful, otherwise (False).</returns>
        public bool TryGetValue<T>(string fieldName, out T? value) where T : IParsable<T>;

        /// <summary>
        /// Record Values.
        /// </summary>
        public string[] Values { get; }
    }

    private static bool typesMatch(Type[] one, Type[] two)
    {
        if (one.Length != two.Length) return false;
        for (int i = 0; i < one.Length; i++)
        {
            if (one[i] != two[i]) return false;
        }
        return true;
    }

    private static Type[] getTypes(string data)
    {
        // possible: DateTime, decimal, else string
        var parts = data.CsvSplit();
        var types = new Type[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            if (DateTime.TryParse(parts[i], out _)) types[i] = typeof(DateTime);
            else if (Decimal.TryParse(parts[i], out _)) types[i] = typeof(Decimal);
            else types[i] = typeof(String);
        }
        return types;
    }
    private List<String> readComments()
    {
        if (_data != null) return new List<string>(_data.TakeWhile(x => !x.Contains(',')).Select(x => x.Trim('\"')));
        return [];
    }

    private class CSVRecord : ICSVRecord
    {
        private readonly CSVDocument _doc;
        private readonly int _index;
        private string[]? _parts;
        public CSVRecord(int index, CSVDocument doc)
        {
            _index = index;
            _doc = doc;
        }

        public string this[int index] => readParts()[index];

        public string this[string fieldName] => this[_doc.Fields.IndexOf(fieldName)];

        public string[] Values => readParts();

        private string[] readParts() => _parts ?? createParts();
        private string[] createParts()
        {
            _parts = _doc._data![_index].CsvSplit().Select(x => x.UnQuote()).ToArray();
            return _parts;
        }

        public bool TryGetValue<T>(int index, out T value) where T : IParsable<T>
            => T.TryParse(readParts()[index], null, out value!);

        public bool TryGetValue<T>(string fieldName, out T value) where T : IParsable<T>
            => TryGetValue(_doc.Fields.IndexOf(fieldName), out value);
    }
}
