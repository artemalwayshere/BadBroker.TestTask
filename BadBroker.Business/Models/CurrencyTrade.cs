using System;

namespace BadBroker.Business.Models
{
    /// <summary>
    /// Currancy trade
    /// </summary>
    public class CurrencyTrade
    {
        /// <summary>
        /// Currancy
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Date of buy
        /// </summary>
        public DateTime BuyDate { get; set; }

        /// <summary>
        /// Date of sell
        /// </summary>
        public DateTime SellDate { get; set; }

        /// <summary>
        /// Revenue
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
