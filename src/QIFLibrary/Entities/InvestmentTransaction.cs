namespace Tudormobile.QIFLibrary.Entities;

/// <summary>
/// Represents an investment transaction.
/// </summary>
public class InvestmentTransaction
{
    /*
     * OFX Schema Example:
     * 
     *    <SELLMF>
     *      <INVSELL>
     *        <INVTRAN>
     *          <FITID>86823817E2055931877</FITID>
     *          <DTTRADE>20240729000000.000</DTTRADE>
     *        </INVTRAN>
     *        <SECID>
     *          <UNIQUEID>LMCSP5$01</UNIQUEID>
     *          <UNIQUEIDTYPE>CUSIP</UNIQUEIDTYPE>
     *        </SECID>
     *        <UNITS>-18.893453</UNITS>
     *        <UNITPRICE>598.365476</UNITPRICE>
     *        <TOTAL>11305.19</TOTAL>
     *        <SUBACCTSEC>OTHER</SUBACCTSEC>
     *        <SUBACCTFUND>OTHER</SUBACCTFUND>
     *      </INVSELL>
     *      <SELLTYPE>SELL</SELLTYPE>
     *    </SELLMF>
     */

    /// <summary>
    /// Unique transaction identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Type of transaction.
    /// </summary>
    public InvestmentTransactionType TransactionType { get; init; }

    /// <summary>
    /// Date posted.
    /// </summary>
    public DateTime DatePosted { get; set; } = default;

    /// <summary>
    /// Security Id.
    /// </summary>
    public string SecurityId { get; init; }

    /// <summary>
    /// Units transacted.
    /// </summary>
    public decimal Units { get; set; }

    /// <summary>
    /// Unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total Amount.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Commission and fees.
    /// </summary>
    public decimal CommissionAndFees => Math.Abs(Total) - (Math.Abs(Units) * UnitPrice);

    /// <summary>
    /// Create and intialize a new instance.
    /// </summary>
    /// <param name="id">Unique identifier for the transaction.</param>
    /// <param name="securityId">Security identifier.</param>
    /// <param name="transactionType">Transaction type.</param>
    /// <param name="datePosted">Date posted</param>
    /// <param name="units">Units transacted.</param>
    /// <param name="unitPrice">Unit price.</param>
    /// <param name="total">Total amount.</param>
    public InvestmentTransaction(string id, string securityId, InvestmentTransactionType transactionType,
        DateTime? datePosted = null,
        decimal units = default,
        decimal unitPrice = default,
        decimal? total = null)
    {
        Id = id;
        SecurityId = securityId;
        TransactionType = transactionType;
        DatePosted = datePosted ?? DateTime.Now;
        Units = units;
        UnitPrice = unitPrice;
        Total = total ?? units * unitPrice;
    }
}

/// <summary>
/// A collection of investment transactions.
/// </summary>
public class InvestmentTransactionList
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
    public IList<InvestmentTransaction> Items { get; } = [];
}

/// <summary>
/// Investment Transaction Types.
/// </summary>
public enum InvestmentTransactionType
{
    /// <summary>
    /// Unknown or unsupported type.
    /// </summary>
    UNKNOWN = 0,
    /// <summary>
    /// Buy Mutual Fund.
    /// </summary>
    BUYMF,
    /// <summary>
    /// Buy Stock.
    /// </summary>
    BUYSTOCK,
    /// <summary>
    /// Sell mutual fund.
    /// </summary>
    SELLMF,
    /// <summary>
    /// Sell stock.
    /// </summary>
    SELLSTOCK,
}
