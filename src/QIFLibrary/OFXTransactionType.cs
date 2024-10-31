using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Transaction type.
/// </summary>
public enum OFXTransactionType
{
    /// <summary>
    /// Other.
    /// </summary>
    OTHER = 0,
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

/// <summary>
/// Extensions methods for transaction types.
/// </summary>
public static class OFXTransactionTypeExtensions
{
    /// <summary>
    /// Description of the transaction type.
    /// </summary>
    /// <param name="transactionType">Transaction type to extend.</param>
    /// <returns>Description of the transaction type, or UNKNOWN.</returns>
    public static string Description(this OFXTransactionType transactionType)
        => transactionType switch
        {
            OFXTransactionType.OTHER => "OTHER",
            OFXTransactionType.CREDIT => "Credit",
            OFXTransactionType.DEBIT => "Debit",
            OFXTransactionType.INT => "Interest",
            OFXTransactionType.DIV => "Dividend",
            OFXTransactionType.FEE => "Fee",
            OFXTransactionType.SRVCHG => "Service Charge",
            OFXTransactionType.DEP => "Deposit",
            OFXTransactionType.ATM => "ATM",
            OFXTransactionType.POS => "Point of Sale",
            OFXTransactionType.XFER => "Transfer",
            OFXTransactionType.CHECK => "Check",
            OFXTransactionType.PAYMENT => "Payment",
            OFXTransactionType.CASH => "Cash",
            OFXTransactionType.DIRECTDEP => "Direct Deposit",
            OFXTransactionType.DIRECTDEBIT => "Direct Debit",
            OFXTransactionType.REPEATPMT => "Repeat Payment",
            _ => "UNKNOWN",
        };
}

