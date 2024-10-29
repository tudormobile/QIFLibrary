using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities;

/// <summary>
/// Represents a financial transaction.
/// </summary>
public class Transaction
{
    /*
        <TRNTYPE>CASH
        <DTPOSTED>20241004000000[-5:EST]
        <TRNAMT>-384.87
        <FITID>0000080152427800000008289151000000038487
        <NAME>ACH Withdrawal
        <MEMO>VIKING RIVER CRU - 8182271234
    */

    /// <summary>
    /// Type of the transaction.
    /// </summary>
    public TransactionTypes TransactionType { get; set; } = TransactionTypes.UNKNOWN;

    /// <summary>
    /// Date posted.
    /// </summary>
    public DateTime DatePosted { get; set; } = default;

    /// <summary>
    /// Amount.
    /// </summary>
    public decimal Amount { get; set; } = default;

    /// <summary>
    /// Unique transaction identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Name of the transaction.
    /// </summary>
    public String Name { get; set; } = String.Empty;

    /// <summary>
    /// Memo.
    /// </summary>
    public String Memo { get; set; } = String.Empty;

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="id">Unique identifier for the transaction.</param>
    public Transaction(string? id = null)
    {
        Id = id ?? new Guid().ToString();
    }

    /// <summary>
    /// Transaction Types.
    /// </summary>
    public enum TransactionTypes
    {
        /// <summary>
        /// Unknown transaction type.
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// Credit Card.
        /// </summary>
        CREDIT,
        /// <summary>
        /// Debit.
        /// </summary>
        DEBIT,
        /// <summary>
        /// Interest.
        /// </summary>
        INT,
        /// <summary>
        /// Dividend.
        /// </summary>
        DIV,
        /// <summary>
        /// Fee.
        /// </summary>
        FEE,
        /// <summary>
        /// Service Charge.
        /// </summary>
        SRVCHG,
        /// <summary>
        /// Deposit.
        /// </summary>
        DEP,
        /// <summary>
        /// ATM.
        /// </summary>
        ATM,
        /// <summary>
        /// Point of sale.
        /// </summary>
        POS,
        /// <summary>
        /// Transfer.
        /// </summary>
        XFER,
        /// <summary>
        /// Check.
        /// </summary>
        CHECK,
        /// <summary>
        /// Payment.
        /// </summary>
        PAYMENT,
        /// <summary>
        /// Cash.
        /// </summary>
        CASH,
        /// <summary>
        /// Direct Deposit.
        /// </summary>
        DIRECTDEP,
        /// <summary>
        /// Direct Debit.
        /// </summary>
        DIRECTDEBIT,
        /// <summary>
        /// Repeat Payment.
        /// </summary>
        REPEATPMT
    }
}

/// <summary>
/// A collection of transactions.
/// </summary>
public class TransactionList
{
    /// <summary>
    /// Start date and time of the transaction period.
    /// </summary>
    public DateTime Start { get; set; } = default;

    /// <summary>
    /// End date and time of the transaction period.
    /// </summary>
    public DateTime End { get; set; } = default;

    /// <summary>
    /// Transaction items.
    /// </summary>
    public IList<Transaction> Items { get; } = [];
}
