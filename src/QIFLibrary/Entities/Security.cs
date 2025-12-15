namespace Tudormobile.QIFLibrary.Entities
{
    /// <summary>
    /// Represents a financial security, such as a stock or a mutual fund.
    /// </summary>
    public class Security
    {
        // allowed types = stock, mutual fund, option, debt, or other.

        /// <summary>
        /// Allow security types.
        /// </summary>
        public enum SecurityTypes
        {
            /// <summary>
            /// Unknown or unset type.
            /// </summary>
            UNKNOWN = 0,

            /// <summary>
            /// Stock.
            /// </summary>
            STOCK,

            /// <summary>
            /// Mutual Fund.
            /// </summary>
            MUTUALFUND,

            /// <summary>
            /// Option.
            /// </summary>
            OPTION,

            /// <summary>
            /// Debt.
            /// </summary>
            DEBT,

            /// <summary>
            /// Other security type.
            /// </summary>
            OTHER
        }

        /// <summary>
        /// Type of this security.
        /// </summary>
        public SecurityTypes SecurityType { get; set; } = SecurityTypes.UNKNOWN;

        /// <summary>
        /// Allow security identifier types.
        /// </summary>
        public enum SecurityIdTypes
        {
            /// <summary>
            /// Unique, application-defined type.
            /// </summary>
            OTHER = 0,

            /// <summary>
            /// Security Id is a ticker symbol.
            /// </summary>
            TICKER = 1,

            /// <summary>
            /// Security identifier is a CUSIP (Committee on Uniform Securities Identification Procedures). 
            /// </summary>
            CUSIP = 2,
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Type of this security identifier.
        /// </summary>
        /// <remarks>
        /// The default value is 'other' unless otherwise known.
        /// </remarks>
        public SecurityIdTypes SecurityIdType { get; set; } = SecurityIdTypes.OTHER;


        /// <summary>
        /// Ticker symbol.
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unit price.
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Unit price date.
        /// </summary>
        public DateTime? UnitPriceDate { get; set; }

        /// <summary>
        /// Create and initialze a new instance.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="ticker">Ticker symbol.</param>
        /// <param name="name">Name.</param>
        /// <param name="unitPrice">Unit price.</param>
        /// <param name="unitPriceDate">Unit price date.</param>
        public Security(string id, string ticker, string name, decimal? unitPrice = null, DateTime? unitPriceDate = null)
        {
            Id = id;
            Ticker = ticker;
            Name = name;
            UnitPrice = unitPrice;
            UnitPriceDate = unitPriceDate;
        }
    }

    /// <summary>
    /// A collection of securities.
    /// </summary>
    public class SecurityList
    {
        /// <summary>
        /// Security items.
        /// </summary>
        public IList<Security> Items { get; } = [];
    }
}
