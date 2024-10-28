using System.Diagnostics;
using System.Globalization;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFXProperty extension methods.
/// </summary>
public static class OFXPropertyExtensions
{
    private static char[] splitChars = ['[', ']', ':'];

    /// <summary>
    /// Determine if a property is empty.
    /// </summary>
    /// <param name="property">Property to extend.</param>
    /// <returns>True if property is empty; otherwise false.</returns>
    public static bool IsEmpty(this OFXProperty property) => string.IsNullOrEmpty(property.Value);

    /// <summary>
    /// Determine if a property has a value.
    /// </summary>
    /// <param name="property">Property to extend.</param>
    /// <returns>True if property is empty; otherwise false.</returns>
    public static bool HasValue(this OFXProperty property) => !property.IsEmpty();

    /// <summary>
    /// Convert a property value to a date.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <param name="defaultValue">Default value.(Optional)</param>
    /// <returns>Converted value if successful; otherwse the default value is returned.</returns>
    /// <remarks>
    /// If no default value is provided and conversion to date is not successful, the current UTC time is returned.
    /// </remarks>
    public static DateTime AsDate(this OFXProperty property, DateTime? defaultValue = null)
    {
        var formats = new string[]
        {
            "yyyyMMddHHmmss",
            "yyyyMMddHHmm",
            "yyyyMMdd",
            "yyyyMMddHHmmss.fff"
        };

        var value = property.Value.Split(splitChars);

        // Try all the valid formats.
        foreach (var format in formats)
        {
            if (DateTime.TryParseExact(value[0].Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
            {
                if (value.Length > 1 && value[1].Trim().Length > 1 && int.TryParse(value[1], out int offset))
                {
                    return result.AddHours(offset);
                }
                return result;
            }
        }
        return defaultValue ?? DateTime.UtcNow;
    }

    /// <summary>
    /// Convert to a integer value.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <param name="defaultValue">Default value to use if unable to convert.</param>
    /// <returns>Property value converted to a integer.</returns>
    public static int AsInteger(this OFXProperty property, int defaultValue = 0)
        => int.TryParse(property.Value, out int result) ? result : defaultValue;

    /// <summary>
    /// Convert to a decimal value.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <param name="defaultValue">Default value to use if unable to convert.</param>
    /// <returns>Property value converted to a decimal.</returns>
    public static decimal AsDecimal(this OFXProperty property, decimal defaultValue = 0m)
    => decimal.TryParse(property.Value, out decimal result) ? result : defaultValue;

    /// <summary>
    /// Convert to a boolean value.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <param name="defaultValue">Default value to use if unable to convert.</param>
    /// <returns>Property value converted to a boolean.</returns>
    public static bool AsBoolean(this OFXProperty property, bool defaultValue = false)
        => property.Value.Length > 0 ? property.Value[0] switch
        {
            '1' => true,
            '0' => false,
            'Y' => true,
            'N' => false,
            'y' => true,
            'n' => false,
            'T' => true,
            'F' => false,
            't' => true,
            'f' => false,
            _ => defaultValue,
        } : defaultValue;

    /// <summary>
    /// Convert to current type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <param name="defaultCurrency">Default value to use.</param>
    /// <returns>Property value converted to the currency type, or the default value if conversion is not successful.</returns>
    public static OFXCurrencyType AsCurrency(this OFXProperty property, OFXCurrencyType defaultCurrency = OFXCurrencyType.USD)
        => Enum.TryParse<OFXCurrencyType>(property.Value, ignoreCase: true, out var currency) ? currency : defaultCurrency;

    /// <summary>
    /// Convert to transaction type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <returns>Property value converted to the transaction type, or 'OTHER' if not successful.</returns>
    public static OFXTransactionType AsTransactionType(this OFXProperty property)
        => Enum.TryParse<OFXTransactionType>(property.Value, ignoreCase: true, out var transactionType) ? transactionType : OFXTransactionType.OTHER;

}
