using System.Security.Claims;
using System.Security.Principal;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Data format of the QIF records.
/// </summary>
public enum QIFDocumentType
{
    /// <summary>
    /// Unknown QIF Data Type.
    /// </summary>
    UNKNOWN = 0,
    /// <summary>
    /// Cash Account
    /// </summary>
    Cash,
    /// <summary>
    /// Checking &amp; Savings Account
    /// </summary>

    Bank,
    /// <summary>
    /// Credit Card Account
    /// </summary>
    CreditCard,
    /// <summary>
    /// Investment Account
    /// </summary>
    Investment,
    /// <summary>
    /// Asset
    /// </summary>
    Asset,
    /// <summary>
    /// Liability
    /// </summary>
    Liability,
    /// <summary>
    /// Invoice
    /// </summary>
    Invoice,
    /// <summary>
    /// Account List
    /// </summary>
    Account,
    /// <summary>
    /// Category list
    /// </summary>
    Category,
    /// <summary>
    /// Class list
    /// </summary>
    Class,
    /// <summary>
    /// Memorized transaction list
    /// </summary>
    Memorized
}

/// <summary>
/// Extension methods for QIFDocumentType
/// </summary>
public static class QIFDocumentTypeExtensions
{
    /// <summary>
    /// 'Friendly' string representation of the data type.
    /// </summary>
    /// <param name="dataType">QIFDataType to extend.</param>
    /// <returns>A string representation of the data type.</returns>
    public static string AsString(this QIFDocumentType dataType) => dataType.Description();

    /// <summary>
    /// Description of the data type.
    /// </summary>
    /// <param name="dataType">QIFDataType to extend.</param>
    /// <returns>A description of the data type.</returns>
    public static string Description(this QIFDocumentType dataType)
    {
        return dataType switch
        {
            QIFDocumentType.Cash => "Cash Account",
            QIFDocumentType.Bank => "Bank Account",
            QIFDocumentType.CreditCard => "Credit Card Account",
            QIFDocumentType.Investment => "Investment Account",
            QIFDocumentType.Asset => "Asset",
            QIFDocumentType.Liability => "Liability",
            QIFDocumentType.Invoice => "Invoice",
            QIFDocumentType.Account => "Account List",
            QIFDocumentType.Category => "Category List",
            QIFDocumentType.Class => "Class list",
            QIFDocumentType.Memorized => "Memorized transaction list",

            _ => "Unknown QIF Data Type",
        };
    }
}
