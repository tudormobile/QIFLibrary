using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities
{
    /// <summary>
    /// Represents a financial security, such as a stock or a mutual fund.
    /// </summary>
    public class Security
    {
        // allowed types = stock, mutual fund, option, debt, or other.

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Ticker symbol.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

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
        public IList<Security> Items { get; } = new List<Security>();
    }
}
