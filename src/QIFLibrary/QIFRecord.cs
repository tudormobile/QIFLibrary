using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Base class for QIF Records.
/// </summary>
/// <param name="date">Date of the item.</param>
/// <param name="amount">Amount of the item.</param>
/// <param name="memo">User entered text to describe the item.</param>
/// <param name="status">Cleared status.</param>
public class QIFRecord(DateTime date, Decimal amount, String memo, String status)
{
    /// <summary>
    /// Date of the item.
    /// </summary>
    public DateTime Date => date;

    /// <summary>
    /// Amount of the item.
    /// </summary>
    public Decimal Amount => amount;

    /// <summary>
    /// User entered text to describe the item.
    /// </summary>
    public String Memo => memo;

    /// <summary>
    /// Cleared status.
    /// </summary>
    public String Status => status;
}
