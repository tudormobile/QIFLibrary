using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities;

/// <summary>
/// Represents an investment position.
/// </summary>
/// <param name="securityId">Unique identifier of the underlying security of this position.</param>
public class Position(string securityId)
{
    /// <summary>
    /// Unique identifier of the underlying security of this position.
    /// </summary>
    public string SecurityId { get; } = securityId;

    /// <summary>
    /// Type of security in this position.
    /// </summary>
    public Security.SecurityTypes SecurityType { get; set; }

    /// <summary>
    /// Position type.
    /// </summary>
    public PositionTypes PositionType { get; set; }

    /// <summary>
    /// Position account type.
    /// </summary>
    public PositionAccountTypes SubAccountType { get; set; }

    /// <summary>
    /// Units.
    /// </summary>
    public decimal Units { get; set; }

    /// <summary>
    /// Unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Market value.
    /// </summary>
    public decimal MarketValue { get; set; }

    /// <summary>
    /// As-of date for the price and value.
    /// </summary>
    public DateTime PriceDate { get; set; } = default;

    /// <summary>
    /// Memo.
    /// </summary>
    public string Memo { get; set; } = string.Empty;

    /// <summary>
    /// Types of positions.
    /// </summary>
    public enum PositionTypes
    {
        /// <summary>
        /// Unknown or unitialized type.
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// Long.
        /// </summary>
        LONG,

        /// <summary>
        /// Short.
        /// </summary>
        SHORT
    }

    /// <summary>
    /// Position account/sub-account types.
    /// </summary>
    public enum PositionAccountTypes
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
}

/// <summary>
/// A collection of positions.
/// </summary>
public class PositionList
{
    /// <summary>
    /// Security items.
    /// </summary>
    public IList<Position> Items { get; } = [];
}

