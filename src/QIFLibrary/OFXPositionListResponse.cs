using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX Position Response message.
/// </summary>
public class OFXPositionListResponse : OFXMessage
{
    private readonly Account _account;
    private readonly PositionList _positions;
    private readonly Lazy<List<OFXProperty>> _properties;

    /// <summary>
    /// OFX Properties.
    /// </summary>
    public override IList<OFXProperty> Properties => _properties.Value;

    /// <summary>
    /// Date as-of for the position list.
    /// </summary>
    public DateTime DateAsOf { get; set; }

    /// <summary>
    /// Currency used
    /// </summary>
    public OFXCurrencyType Currency { get; set; }

    /// <summary>
    /// Cookie
    /// </summary>
    public string Cookie { get; set; }

    /// <summary>
    /// Create and initialize a new instance.
    /// </summary>
    /// <param name="positions">Positions.</param>
    /// <param name="account">Account.</param>
    /// <param name="dateAsOf">Date as-of. (optional; defaul is NOW)</param>
    /// <param name="id">Unique identifier (optional)</param>
    /// <param name="cookie">Cookie (optional)</param>
    /// <param name="status">Status (optional; default is INFO/code 0)</param>
    /// <param name="currency">Currency (optional; default is USD).</param>
    public OFXPositionListResponse(PositionList positions, Account account, DateTime? dateAsOf = null, string id = "", string cookie = "", OFXStatus? status = null, OFXCurrencyType currency = OFXCurrencyType.USD)
    {
        _positions = positions;
        _account = account;

        DateAsOf = dateAsOf ?? DateTime.Now;
        Currency = currency;
        Id = id;
        Status = status ?? new OFXStatus() { Code = 0, Severity = OFXStatus.StatusSeverity.INFO };
        Cookie = cookie;

        _properties = new Lazy<List<OFXProperty>>(generateProperties);
        Name = "INVSTMTTRNRS";
    }

    private List<OFXProperty> generateProperties()
    {
        var result = new List<OFXProperty>();
        if (!string.IsNullOrEmpty(Cookie)) { result.Add(new OFXProperty("CLTCOOKIE", Cookie)); }

        var invPosList = new OFXInvestmentPositionList(_positions);

        var invResponse = new OFXProperty("INVSTMTRS");
        invResponse.Children.Add(DateAsOf, "ASOF").Add(Currency).Add(_account, OFXMessageDirection.RESPONSE);
        invResponse.Children.Add(invPosList);

        result.Add(invResponse);
        return result;
    }
}
