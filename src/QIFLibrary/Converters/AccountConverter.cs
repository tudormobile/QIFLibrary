using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary.Converters;

/// <inheritdoc/>
public class AccountConverter : PropertyConverterBase<Account>
{
    /// <summary>
    /// Key property for this entity.
    /// </summary>
    public static readonly string KEY = "INVACCTFROM|INVACCTTO|BANKACCTFROM|BANKACCTTO|CCACCTFROM|CCACCTTO";

    /// <inheritdoc/>
    public override Account? Convert(OFXProperty root)
    {
        foreach (var key in KEY.Split('|'))
        {
            var p = DigForProperty(root, key);
            if (p != null)
            {
                var isBroker = key.StartsWith("INV");
                return new Account()
                {
                    AccountId = p.Children["ACCTID"].Value,
                    InstitutionId = isBroker ? p.Children["BROKERID"].Value : p.Children["BANKID"].Value,
                    AccountType = isBroker ? Account.AccountTypes.INVESTMENT : (Account.AccountTypes)p.Children["ACCTTYPE"].AsAccountType(),
                };
            }
        }
        return default;
    }
}

/// <inheritdoc/>
public static partial class OFXPropertyConverterExtensions
{
    /// <summary>
    /// Converts OFX property to account.
    /// </summary>
    /// <param name="converter">Converter to extend.</param>
    /// <param name="root">Root property.</param>
    /// <returns>Account.</returns>
    public static Account? GetAccount(this OFXPropertyConverter converter, OFXProperty root)
        => new AccountConverter().Convert(root);
}

