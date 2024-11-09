using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary
{
    /// <summary>
    /// Investment Statement Response Message.
    /// </summary>
    public class OFXInvestmentStatementResponse : OFXMessage
    {
        private readonly OFXPropertyConverter _converter;
        private readonly Lazy<OFXProperty> _invstmtrsProp;
        private readonly Lazy<Account> _account;
        private readonly Lazy<PositionList> _positionList;
        private readonly Lazy<InvestmentTransactionList> _investmentTransactionList;

        /// <summary>
        /// Cookie value.
        /// </summary>
        public string Cookie => Properties.FirstOrDefault(p => p.Name == "CLTCOOKIE")?.Value ?? String.Empty;

        /// <summary>
        /// Currency type.
        /// </summary>
        public OFXCurrencyType Currency => _invstmtrsProp.Value.Children["CURDEF"].AsCurrency();

        /// <summary>
        /// As-Of date of this satement.
        /// </summary>
        public DateTime Date => _invstmtrsProp.Value.Children["DTASOF"].AsDate();

        /// <summary>
        /// Account.
        /// </summary>
        public Account Account => _account.Value;

        /// <summary>
        /// Transaction List.
        /// </summary>
        public InvestmentTransactionList TransactionList => _investmentTransactionList.Value;

        /// <summary>
        /// Position list.
        /// </summary>
        public PositionList PositionList => _positionList.Value;

        /// <summary>
        /// 401K Total Balance (optional).
        /// </summary>
        public decimal? Investment401kBalance => _invstmtrsProp.Value.Children.FirstOrDefault(c => c.Name == "INV401KBAL")?.Children["TOTAL"].AsDecimal();

        /// <summary>
        /// Create and initialize a new OFXInvestment Statement Response.
        /// </summary>
        public OFXInvestmentStatementResponse()
        {
            _converter = new OFXPropertyConverter();

            _invstmtrsProp = new Lazy<OFXProperty>(() => Properties.FirstOrDefault(p => p.Name == "INVSTMTRS") ?? new OFXProperty("INVSTMTRS"));
            _account = new(() => _converter.GetAccount(_invstmtrsProp.Value.Children["INVACCTFROM"])!);
            _positionList = new(() => _converter.GetPositionList(_invstmtrsProp.Value.Children["INVPOSLIST"])!);
            _investmentTransactionList = new(() => _converter.GetInvestmentTransactionList(_invstmtrsProp.Value.Children["INVTRANLIST"])!);
        }

    }
}
