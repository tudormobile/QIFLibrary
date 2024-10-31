using System.Data;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Represents a QIF Investment record.
/// </summary>
public class QIFInvestment : QIFRecord
{
    /// <summary>
    /// An empty (invalid) record.
    /// </summary>
    public static new readonly QIFInvestment Empty = new();

    /// <summary>
    /// Check Number.
    /// </summary>
    public string Check { get; }

    /// <summary>
    /// Payee
    /// </summary>
    public string Payee { get; }

    /// <summary>
    /// Address of Payee.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Category or Transfer and (optionally) Class.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Investment Action (Buy, Sell, etc.).
    /// </summary>
    public QIFInvestmentType InvestmentAction { get; }

    /// <summary>
    /// Security name.
    /// </summary>
    public string SecurityName { get; }

    /// <summary>
    /// Quantity of shares.
    /// </summary>
    public decimal Quantity { get; }

    /// <summary>
    /// Price.
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// Commission cost.
    /// </summary>
    public decimal Commision { get; }

    /// <summary>
    /// Amount Transferred.
    /// </summary>
    public decimal SplitAmount { get; }

    private QIFInvestment() : this(DateTime.MinValue, 0, String.Empty, String.Empty,
        String.Empty,
        String.Empty,
        String.Empty,
        String.Empty,
        QIFInvestmentType.UNKNOWN,
        String.Empty, 0, 0, 0, 0)
    { }

    /// <summary>
    /// Create and initialize a new QIF Investment Transaction.
    /// </summary>
    /// <param name="Date">Transction date.</param>
    /// <param name="Amount">Amount of the transaction.</param>
    /// <param name="Memo">User supplied memo.</param>
    /// <param name="Status">Cleared status of the transaction.</param>
    /// <param name="check">Check number.</param>
    /// <param name="payee">Payee.</param>
    /// <param name="address">Address of payee.</param>
    /// <param name="category">Category.</param>
    /// <param name="investmentType">Investment Transaction type.</param>
    /// <param name="securityName">Security name.</param>
    /// <param name="price">Price.</param>
    /// <param name="quantity">Quantity.</param>
    /// <param name="commision">Commision cost.</param>
    /// <param name="splitAmount">Amount Transferred</param>
    public QIFInvestment(DateTime Date, decimal Amount, string Memo, string Status,
        string check,
        string payee,
        string address,
        string category,
        QIFInvestmentType investmentType,
        string securityName,
        decimal price,
        decimal quantity,
        decimal commision,
        decimal splitAmount) : base(Date, Amount, Memo, Status)
    {
        this.Check = check;
        this.Payee = payee;
        this.Address = address;
        this.Category = category;
        this.InvestmentAction = investmentType;
        this.SecurityName = securityName;
        this.Price = price;
        this.Quantity = quantity;
        this.Commision = commision;
        this.SplitAmount = splitAmount;
    }

    /// <summary>
    /// Converts this transaction to a string.
    /// </summary>
    /// <returns>String representation of this transaction.</returns>
    public override string ToString() => $"Investment={SecurityName}, Action={InvestmentAction.Description()}, Shares = {Quantity}, Price = ${Price}, Amount = ${Amount}";

}
