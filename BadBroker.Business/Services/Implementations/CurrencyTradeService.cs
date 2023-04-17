using BadBroker.Business.Models;
using BadBroker.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadBroker.Business.Services.Implementations
{
    public class CurrencyTradeService : ICurrencyTradeService
    {
        /// <summary>
        /// Get the best trade
        /// </summary>
        /// <param name="rates"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="moneyUsd"></param>
        /// <returns></returns>
        public CurrencyTrade Calculate(IList<Rate> rates, DateTime startDate, DateTime endDate, decimal moneyUsd)
        {
            var brokerFee = 1;
            if ((endDate - startDate).TotalDays > 60)
            {
                throw new Exception("The maximum possible period is 60 days");
            }

            var validCurrencies = new List<string> { "RUB", "EUR", "GBP", "JPY" };

            var validRates = rates.Where(rate => validCurrencies.Contains(rate.CurrencySymbol) && rate.Date >= startDate && rate.Date <= endDate);

            if (!validRates.Any())
                throw new ArgumentException("No valid currency rates found in the specified period.");

            var revenue = 0m;
            var buyCurrency = string.Empty;
            var buyDate = DateTime.MinValue;
            var sellDate = DateTime.MinValue;

            foreach (var buyRate in validRates)
            {
                var currencyAmount = moneyUsd / (buyRate.Value + brokerFee);

                var sellRate = validRates
                    .Where(rate => rate.CurrencySymbol == buyRate.CurrencySymbol && rate.Date > buyRate.Date)
                    .OrderBy(rate => rate.Value)
                    .FirstOrDefault();

                if (sellRate != null)
                {
                    var revenueUsd = (currencyAmount * sellRate.Value) - 1;

                    if (revenueUsd > revenue)
                    {
                        revenue = revenueUsd;
                        buyCurrency = buyRate.CurrencySymbol;
                        buyDate = buyRate.Date;
                        sellDate = sellRate.Date;
                    }
                }
            }

            if (revenue == 0m)
                throw new ArgumentException("No profitable trades found in the specified period.");

            return new CurrencyTrade()
            {
                Currency = buyCurrency,
                BuyDate = buyDate,
                SellDate = sellDate,
                Revenue = revenue
            };
        }
    }
}

