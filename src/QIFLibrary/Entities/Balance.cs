using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities
{
    /// <summary>
    /// Provides mechanism for storing balances.
    /// </summary>
    /// <param name="value">Balance value.</param>
    /// <param name="date">Balance date (optional).</param>
    public readonly struct Balance(decimal value, DateTime? date = null)
    {
        /// <summary>
        /// Implicit conversion to decimal.
        /// </summary>
        /// <param name="balances">Primary balance value.</param>
        public static implicit operator decimal(Balance balances) => balances.Value;

        /// <summary>
        /// Primary value.
        /// </summary>
        public decimal Value => value;

        /// <summary>
        /// Balance date, if available.
        /// </summary>
        public DateTime? Date => date;
    }
}
