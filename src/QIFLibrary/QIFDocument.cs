using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides mechanism for reading and writing data in Quicken Interchange Format (QIF).
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
    /// <param name="leaveOpen">Leave the provided stream open [OPTONAL; Default = true].</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    public static QIFDocument Parse(Stream utf8Stream, bool leaveOpen = true) => parse(new StreamReader(utf8Stream, Encoding.UTF8), leaveOpen);

    /// <summary>
    /// Parses data into a QIF Document.
    /// </summary>
    /// <param name="qifData">QIF text to parse.</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    public static QIFDocument Parse(string qifData) => parse(new StringReader(qifData));

    /// <summary>
    /// Parses data into a QIF Document.
    /// </summary>
    /// <param name="path">The path to the file to be parsed.</param>
    /// <returns>A QIFDocument representation of the data.</returns>
    public static QIFDocument ParseFile(string path) => parse(File.OpenText(path));

    private static QIFDocument parse(TextReader reader, bool leaveOpen = false)
    {
        try
        {
            var header = dataTypeFromHeader(reader.ReadLine());
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

    internal static QIFDocumentType dataTypeFromHeader(string? headerLine) => headerLine?.Trim() switch
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
        _ => QIFDocumentType.UNKNOWN,
    };

}
