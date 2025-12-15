using System.Text;
using Tudormobile.QIFLibrary.IO;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanism for reading and writing data in Quicken Interchange Format (QIF/QFX).
/// </summary>
public sealed class QIFDocument
{
    /// <summary>
    /// The data type of the QIF records in this document.
    /// </summary>
    public QIFDocumentType DataType { get; }

    /// <summary>
    /// QIF Data records contained in this document.
    /// </summary>
    public IList<QIFRecord> Records { get; }

    private QIFDocument(QIFDocumentType dataType, List<QIFRecord> records)
    {
        this.DataType = dataType;
        this.Records = records;
    }

    /// <summary>
    /// Parses data into a QIF Document.
    /// </summary>
    /// <param name="utf8Stream">QIF text to parse.</param>
    /// <param name="leaveOpen">Leave the provided stream open [OPTIONAL; Default = true].</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="utf8Stream"/> is null.</exception>
    public static QIFDocument Parse(Stream utf8Stream, bool leaveOpen = true)
    {
        ArgumentNullException.ThrowIfNull(utf8Stream);
        return Parse(new StreamReader(utf8Stream, Encoding.UTF8), leaveOpen);
    }

    /// <summary>
    /// Parses data into a QIF Document.
    /// </summary>
    /// <param name="qifData">QIF text to parse.</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    public static QIFDocument Parse(string qifData) => Parse(new StringReader(qifData));

    /// <summary>
    /// Parses data into a QIF Document.
    /// </summary>
    /// <param name="path">The path to the file to be parsed.</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    public static QIFDocument ParseFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        using var reader = File.OpenText(path);
        return Parse(reader, leaveOpen: false);
    }

    private static QIFDocument Parse(TextReader reader, bool leaveOpen = false)
    {
        try
        {
            var header = DataTypeFromHeader(reader.ReadLine());
            var records = new List<QIFRecord>();
            var qifReader = new QIFReader(reader);

            while (qifReader.ReadRecord(header, out var record))
            {
                records.Add(record!);
            }

            return new QIFDocument(header, records);
        }
        finally
        {
            if (!leaveOpen)
            {
                reader.Close();
            }
        }
    }

    internal static QIFDocumentType DataTypeFromHeader(string? headerLine) => headerLine?.Trim() switch
    {
        "!Type:Cash" => QIFDocumentType.Cash,
        "!Type:Bank" => QIFDocumentType.Bank,
        "!Type:CCard" => QIFDocumentType.CreditCard,
        "!Type:Invst" => QIFDocumentType.Investment,
        "!Type:Oth A" => QIFDocumentType.Asset,
        "!Type:Oth L" => QIFDocumentType.Liability,
        "!Type:Invoice" => QIFDocumentType.Invoice,
        "!Type:Account" => QIFDocumentType.Account,
        "!Type:Cat" => QIFDocumentType.Category,
        "!Type:Class" => QIFDocumentType.Class,
        "!Type:Memorized" => QIFDocumentType.Memorized,
        "!Account" => QIFDocumentType.Account,
        _ => QIFDocumentType.UNKNOWN,
    };

}
