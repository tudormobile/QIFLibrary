using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.IO;

/// <summary>
/// Provides mechanisms to write CSV data.
/// </summary>
public class CSVWriter
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="writer">Text writer to use as the data sink.</param>
    public CSVWriter(TextWriter writer)
    {
        _writer = writer;
    }

    /// <summary>
    /// Writes a document.
    /// </summary>
    /// <param name="document">Document to write.</param>
    public void Write(CSVDocument document)
    {
        WriteComments(document.Comments);
        if (document.Fields != null)
        {
            _writer.WriteLine(string.Join(",", document.Fields));
        }
        WriteRecords(document.Records);
    }

    /// <summary>
    /// Write records to output.
    /// </summary>
    /// <param name="records">Records to write.</param>
    public void WriteRecords(IEnumerable<CSVDocument.ICSVRecord> records)
        => writeLines(records.Select(x => toCSV(x)));

    /// <summary>
    /// Write comments to output.
    /// </summary>
    /// <param name="comments">comments to write.</param>
    /// <remarks>
    /// Any commas that appear in the comments are replaced with a space.
    /// </remarks>
    public void WriteComments(IEnumerable<string> comments)
        => writeLines(comments.Select(c => c.Replace(',', ' ')));

    /// <summary>
    /// Write a record to output.
    /// </summary>
    /// <param name="record">Record to write.</param>
    public void WriteRecord(CSVDocument.ICSVRecord record) => WriteRecords([record]);

    /// <summary>
    /// Write a comment to output.
    /// </summary>
    /// <param name="comment">Comment to write.</param>
    public void WriteComment(string comment) => WriteComments([comment]);

    private static string toCSV(CSVDocument.ICSVRecord record)
        => string.Join(",", record.Values.Select(x => sanitize(x)));

    private void writeLines(IEnumerable<string> linesToWrite)
    {
        _writer.NewLine = "\r\n";
        foreach (var line in linesToWrite)
        {
            _writer.WriteLine(line);
        }
    }

    private static string sanitize(string input)
    {
        if (input.Contains(',') || input.Contains('\"'))
        {
            return $"\"{input}\"";
        }
        return input;
    }

}
