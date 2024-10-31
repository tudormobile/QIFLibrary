using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Currency Types.
/// </summary>
/// <remarks>
/// Ref. ISO-4217.
/// </remarks>
public enum OFXCurrencyType
{
    /// <summary>
    /// Unknown currency type.
    /// </summary>
    UNKNOWN = 0,

    /// <summary>
    /// US Dollar.
    /// </summary>
    USD = 840,

    /// <summary>
    /// Euro
    /// </summary>
    EUR = 978,

    /// <summary>
    /// Canadian Dollar.
    /// </summary>
    CAD = 124,

    /// <summary>
    /// Austrailian Dollar
    /// </summary>
    AUD = 36,

    /// <summary>
    /// Swiss franc.
    /// </summary>
    CHF = 756,

    /// <summary>
    /// British Pound Sterling.
    /// </summary>
    GBP = 826,

    /// <summary>
    /// Mexican Peso.
    /// </summary>
    MXN = 484,

    /// <summary>
    /// Japeneese Yen.
    /// </summary>
    JPY = 392,

}

/// <summary>
/// Extensions methods for currency types.
/// </summary>
public static class OFXCurrencyTypeExtensions
{
    /// <summary>
    /// Description of the current type.
    /// </summary>
    /// <param name="cur">Currency type to extend.</param>
    /// <returns>Description of the currency type, or UNKNOWN.</returns>
    public static string Description(this OFXCurrencyType cur)
        => cur switch
        {
            OFXCurrencyType.USD => "US dollar",
            OFXCurrencyType.EUR => "Euro",
            OFXCurrencyType.MXN => "Mexican peso",
            OFXCurrencyType.GBP => "British pound sterling",
            OFXCurrencyType.CAD => "Canadian dollar",
            OFXCurrencyType.CHF => "Swiss franc",
            OFXCurrencyType.JPY => "Japanese yen",
            OFXCurrencyType.AUD => "Australian dollar",
            _ => "UNKNOWN"
        };
}
