using System;

namespace BadBroker.Business.Models
{
    /// <summary>
    /// Currancy rate per date
    /// </summary>
    public class Rate
    {
        /// <summary>
        /// Symbol of currancy
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Rate by currancy
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
