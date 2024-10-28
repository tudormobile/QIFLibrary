using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Types if accounts.
/// </summary>
public enum OFXAccountType
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
