using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Represents a QIF Bank record.
/// </summary>
public class QIFBankRecord : QIFRecord
{
    /// <summary>
    /// Description of the banking transaction.
    /// </summary>
    public String Description { get; }

    /// <summary>
    /// Category or Transfer and (optionally) Class.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Address of Payee.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Check Number.
    /// </summary>
    public string CheckNo { get; }

    /// <summary>
    /// True if transaction is flagged, otherwise false.
    /// </summary>
    public bool Flagged { get; }

    /// <summary>
    /// Create and initialze a new Banking record.
    /// </summary>
    /// <param name="date">Transction date.</param>
    /// <param name="amount">Amount of the transaction.</param>
    /// <param name="memo">User supplied memo.</param>
    /// <param name="status">Cleared status of the transaction.</param>
    /// <param name="description">Description of the banking transaction.</param>
    /// <param name="category">Category or Transfer and (optionally) Class.</param>
    /// <param name="address">Address of Payee.</param>
    /// <param name="checkNo">Check Number.</param>
    /// <param name="flagged">True if transaction is flagged.</param>
    public QIFBankRecord(DateTime date, decimal amount, string memo, string status,
        string description, string category, string address, string checkNo, bool flagged = false)
        : base(date, amount, memo, status)
    {
        Description = description;
        Category = category;
        Address = address;
        CheckNo = checkNo;
        Flagged = flagged;
    }
}
