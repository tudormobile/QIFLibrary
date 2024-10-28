using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities;

/// <summary>
/// An Account.
/// </summary>
public class Account
{
    /// <summary>
    /// Account number or other unique identifier for the account.
    /// </summary>
    public string AccountId { get; set; } = string.Empty;

    /// <summary>
    /// Bank or broker institution identifier.
    /// </summary>
    public string InstitutionId { get; set; } = String.Empty;

    /// <summary>
    /// Type of account.
    /// </summary>
    public AccountTypes AccountType { get; set; } = AccountTypes.UNKNOWN;

    /// <summary>
    /// Types if accounts.
    /// </summary>
    public enum AccountTypes
    {
        /// <summary>
        /// Unknown account type.
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// Investment account.
        /// </summary>
        INVESTMENT,

        /// <summary>
        /// Checking account.
        /// </summary>
        CHECKING,

        /// <summary>
        /// Savings account.
        /// </summary>
        SAVINGS,

        /// <summary>
        /// Money market account.
        /// </summary>
        MONEYMRKT,

        /// <summary>
        /// Credit line account.
        /// </summary>
        CREDITLINE
    }
}
