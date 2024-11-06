using System.Globalization;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFXProperty extension methods.
/// </summary>
public static class OFXPropertyExtensions
{
    private static readonly char[] splitChars = ['[', ']', ':'];

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
    /// Determine if a property has children.
    /// </summary>
    /// <param name="property">Property to extend.</param>
    /// <returns>True if property has children; otherwise false.</returns>
    public static bool HasChildren(this OFXProperty property) => property.Children.Count > 0;

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

    /// <summary>
    /// Convert to language type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <returns>Property value converted to the transaction type, or 'UNKNOWN' if not successful.</returns>
    public static OFXLanguage AsLanguage(this OFXProperty property)
        => Enum.TryParse<OFXLanguage>(property.Value, ignoreCase: true, out var transactionType) ? transactionType : OFXLanguage.UNKNOWN;

    /// <summary>
    /// Convert to account type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <returns>Property value converted to the account type, or 'UNKNOWN' if not successful.</returns>
    public static OFXAccountType AsAccountType(this OFXProperty property)
        => Enum.TryParse<OFXAccountType>(property.Value, ignoreCase: true, out var accountType) ? accountType : OFXAccountType.UNKNOWN;

    /// <summary>
    /// Convert to position account type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <returns>Property value converted to the position account type, or 'OTHER' if not successful.</returns>
    public static OFXPositionAccountTypes AsPositionAccountType(this OFXProperty property)
        => Enum.TryParse<OFXPositionAccountTypes>(property.Value, ignoreCase: true, out var positionAccountType) ? positionAccountType : OFXPositionAccountTypes.OTHER;

    /// <summary>
    /// Convert to position type.
    /// </summary>
    /// <param name="property">Property to convert.</param>
    /// <returns>Property value converted to the position type, or 'UNKNOWN' if not successful.</returns>
    public static OFXPositionTypes AsPositionType(this OFXProperty property)
        => Enum.TryParse<OFXPositionTypes>(property.Value, ignoreCase: true, out var positionType) ? positionType : OFXPositionTypes.UNKNOWN;

    /// <summary>
    /// Add language to a property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="language">Language to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, OFXLanguage language)
    {
        list.Add(new OFXProperty("LANGUAGE", language.ToString()));
        return list;
    }

    /// <summary>
    /// Add currency to the property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="currency">Currency to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, OFXCurrencyType currency = OFXCurrencyType.USD)
    {
        list.Add(new OFXProperty("CURDEF", currency.ToString()));
        return list;
    }

    /// <summary>
    /// Add a decimal value to the property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="value">Value to add.</param>
    /// <param name="valueName">Name of the value property.</param>
    /// <param name="rounding">(OPTIONAL) Round to 2-placed using this strategy.</param>
    /// <returns>Fluent reference to the list.</returns>
    /// <remarks>
    /// No rounding is performed unless a rounding strategy is provided. Note that "rounding to even" is typically used by bankers.
    /// </remarks>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, decimal value, string valueName, MidpointRounding? rounding = null)
    {
        var val = rounding == null ? value : Math.Round(value, 2, rounding.Value);
        list.Add(new OFXProperty(valueName, val.ToString()));
        return list;
    }

    /// <summary>
    /// Add a position account type to the property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="accountType">Position account type to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, Position.PositionAccountTypes accountType)
    {
        list.Add(new OFXProperty("HELDINACCT", accountType.ToString()));
        return list;
    }

    /// <summary>
    /// Add a position type to the property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="positionType">Position type to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, Position.PositionTypes positionType)
    {
        list.Add(new OFXProperty("POSTYPE", positionType.ToString()));
        return list;
    }

    /// <summary>
    /// Add account to property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="account">Account to add.</param>
    /// <param name="direction"></param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, Account account, OFXMessageDirection direction = OFXMessageDirection.RESPONSE)
    {
        var type = account.AccountType switch
        {
            Account.AccountTypes.CREDITLINE => "CC",
            Account.AccountTypes.INVESTMENT => "INV",
            _ => "BANK"
        };
        var tofrom = direction == OFXMessageDirection.RESPONSE ? "FROM" : "TO";
        var prop = new OFXProperty($"{type}ACCT{tofrom}");
        prop.Children.Add(new OFXProperty("BROKERID", account.InstitutionId));
        prop.Children.Add(new OFXProperty("ACCTID", account.AccountId));

        list.Add(prop);
        return list;
    }

    /// <summary>
    /// Add financial institution to a property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="institution">Financial institution to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, Institution institution)
    {
        var prop = new OFXProperty("FI");
        prop.Children.Add(new OFXProperty("ORG", institution.Name));
        prop.Children.Add(new OFXProperty("FID", institution.Id));
        list.Add(prop);
        return list;
    }

    /// <summary>
    /// Add a date to a property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="date">Date to add.</param>
    /// <param name="dateType">Type of the date.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, DateTime date, string dateType)
    {
        list.Add(new OFXProperty($"DT{dateType}", date.ToUniversalTime().ToString("yyyyMMddHHmmss")));
        return list;
    }

    /// <summary>
    /// Add investment position list to a property list.
    /// </summary>
    /// <param name="list">List to extend.</param>
    /// <param name="positions">Investment position list to add.</param>
    /// <returns>Fluent reference to the list.</returns>
    public static IList<OFXProperty> Add(this IList<OFXProperty> list, PositionList positions)
    {
        list.Add(new OFXInvestmentPositionList(positions));
        return list;
    }

}
