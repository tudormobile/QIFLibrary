using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters;

/// <inheritdoc/>
public class AccountConverter : PropertyConverterBase<Account>
{
    /// <summary>
    /// Key property for this entity.
    /// </summary>
    public static string KEY = "INVACCTFROM|INVACCTTO|BANKACCTFROM|BANKACCTTO|CCACCTFROM|CCACCTTO";

    /// <inheritdoc/>
    public override Account? Convert(OFXProperty root)
    {
        foreach (var key in KEY.Split('|'))
        {
            var p = digForProperty(root, key);
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

