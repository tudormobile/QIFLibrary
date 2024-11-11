namespace Tudormobile.QIFLibrary.IO;

/// <summary>
/// Reads data in Quicken Interchange Format (QIF).
/// </summary>
public class QIFReader
{
    private readonly TextReader _reader;
    private QIFRecordBuilder? _builder;
    internal QIFReader(TextReader reader) { _reader = reader; }

    /// <summary>
    /// Create and initialize a new QIFReader.
    /// </summary>
    /// <param name="stream">Stream to read.</param>
    /// <returns>A new QIF reader.</returns>
    /// <remarks>
    /// Call Close() when finished with the reader, which will also close the provided stream.
    /// </remarks>
    public static QIFReader FromStream(Stream stream) => new QIFReader(new StreamReader(stream));

    /// <summary>
    /// Reads a record asychronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public async Task<QIFRecord> ReadRecordAsync()
    {
        if (_builder == null)
        {
            var headerLine = await _reader.ReadLineAsync();
            _builder = new QIFRecordBuilder(QIFDocument.dataTypeFromHeader(headerLine));
        }
        _builder.Clear();

        string? line = await _reader.ReadLineAsync();
        if (line == null) return QIFRecord.Empty;
        while (line != null && line != "^")
        {
            _builder.Add(line);
            line = await _reader.ReadLineAsync();
        }
        return _builder.Build();
    }

    /// <summary>
    /// Enumerates records asynchronously.
    /// </summary>
    /// <returns>An enumerator for iterating records asynchronously.</returns>
    public async IAsyncEnumerable<QIFRecord> ReadRecordsAsync()
    {
        QIFRecord record;
        while ((record = await ReadRecordAsync()).Date != DateTime.MinValue)
        {
            yield return record;
        }
        if (record is QIFCategoryRecord)
        {
            yield return record;    // support only 1 category record? I do not know the category QIF format.
        }
    }

    /// <summary>
    /// Closes the reader and the underlying stream.
    /// </summary>
    public void Close() => _reader.Close();

    internal bool ReadRecord(QIFDocumentType header, out QIFRecord? record)
    {
        if (_builder == null)
        {
            _builder = new QIFRecordBuilder(header);
        }
        _builder.Clear();
        string? line = _reader.ReadLine();
        do
        {
            if (line?.Trim() == "^")
            {
                record = _builder.Build();
                return true;
            }
            else if (line != null)
            {
                _builder.Add(line);
            }
            line = _reader.ReadLine();
        } while (line != null);
        record = null;
        return false;
    }

}