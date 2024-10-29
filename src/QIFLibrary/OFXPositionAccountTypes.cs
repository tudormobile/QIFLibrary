using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Position account/sub-account types.
/// </summary>
public enum OFXPositionAccountTypes
{
    /// <summary>
    /// Other or N/A position account/sub-account type.
    /// </summary>
    OTHER = 0,

    /// <summary>
    /// Cash account.
    /// </summary>
    CASH,

    /// <summary>
    /// Margin account.
    /// </summary>
    MARGIN,

    /// <summary>
    /// Short/Cover account.
    /// </summary>
    SHORT
}