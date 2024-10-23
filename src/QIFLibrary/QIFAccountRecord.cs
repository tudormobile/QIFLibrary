using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Represents a QIF Account record.
/// </summary>
public class QIFAccountRecord : QIFRecord
{
    /// <summary>
    /// Account name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Account description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Account type.
    /// </summary>
    public string AccountType { get; }

    /// <summary>
    /// Account balance.
    /// </summary>
    public decimal Balance { get; }

    /// <summary>
    /// Account credit limit.
    /// </summary>
    public decimal CreditLimit { get; }

    /// <summary>
    /// Creates and initializes a new QIF Account record.
    /// </summary>
    /// <param name="name">Account name.</param>
    /// <param name="description">Account description.</param>
    /// <param name="accountType">Account type.</param>
    /// <param name="balance">Account balance.</param>
    /// <param name="creditLimit">Account credit limit.</param>
    public QIFAccountRecord(string name, string description, string accountType, decimal balance = 0, decimal creditLimit = 0)
        : base(DateTime.Now, 0, description, String.Empty)
    {
        Name = name;
        Description = description;
        AccountType = accountType;
        Balance = balance;
        CreditLimit = creditLimit;
    }
}
