using BadBroker.Business.Models;
using BadBroker.Business.Services.Implementations;
using BadBroker.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BadBroker.Tests
{
    public class CurrencyTradeServiceTests
    {
        List<Rate> rates = new List<Rate>
            {
                new Rate { CurrencySymbol = "RUB", Value = 0.02m, Date = new DateTime(2023, 4, 1) },
                new Rate { CurrencySymbol = "RUB", Value = 0.03m, Date = new DateTime(2023, 4, 3) },
                new Rate { CurrencySymbol = "RUB", Value = 0.04m, Date = new DateTime(2023, 4, 5) },
                new Rate { CurrencySymbol = "EUR", Value = 1.2m, Date = new DateTime(2023, 4, 1) },
                new Rate { CurrencySymbol = "EUR", Value = 1.3m, Date = new DateTime(2023, 4, 3) },
                new Rate { CurrencySymbol = "EUR", Value = 1.4m, Date = new DateTime(2023, 4, 5) }
            };

        [Fact]
        public void Calculate_ThrowsException_WhenInvalidPeriod()
        {
            // Arrange
            var startDate = new DateTime(2022, 01, 01);
            var endDate = new DateTime(2023, 03, 01);
            var moneyUsd = 100m;
            var currencyTradeService = new CurrencyTradeService();

            // Act & Assert
            Assert.Throws<Exception>(() => currencyTradeService.Calculate(rates, startDate, endDate, moneyUsd));
        }

        [Fact]
        public void Calculate_Returns_Correct_Trade()
        {
            // Arrange
            var service = new CurrencyTradeService();

            var rates = new List<Rate>
            {
                new Rate { CurrencySymbol = "RUB", Date = new DateTime(2023, 04, 01), Value = 75.0m },
                new Rate { CurrencySymbol = "EUR", Date = new DateTime(2023, 04, 01), Value = 0.85m },
                new Rate { CurrencySymbol = "GBP", Date = new DateTime(2023, 04, 01), Value = 0.73m },
                new Rate { CurrencySymbol = "JPY", Date = new DateTime(2023, 04, 01), Value = 110.0m },
                new Rate { CurrencySymbol = "RUB", Date = new DateTime(2023, 04, 03), Value = 74.0m },
                new Rate { CurrencySymbol = "EUR", Date = new DateTime(2023, 04, 03), Value = 0.84m },
                new Rate { CurrencySymbol = "GBP", Date = new DateTime(2023, 04, 03), Value = 0.72m },
                new Rate { CurrencySymbol = "JPY", Date = new DateTime(2023, 04, 03), Value = 109.0m },
                new Rate { CurrencySymbol = "RUB", Date = new DateTime(2023, 04, 05), Value = 73.0m },
                new Rate { CurrencySymbol = "EUR", Date = new DateTime(2023, 04, 05), Value = 0.83m },
                new Rate { CurrencySymbol = "GBP", Date = new DateTime(2023, 04, 05), Value = 0.71m },
                new Rate { CurrencySymbol = "JPY", Date = new DateTime(2023, 04, 05), Value = 108.0m },
                new Rate { CurrencySymbol = "RUB", Date = new DateTime(2023, 04, 07), Value = 72.0m },
                new Rate { CurrencySymbol = "EUR", Date = new DateTime(2023, 04, 07), Value = 0.82m },
                new Rate { CurrencySymbol = "GBP", Date = new DateTime(2023, 04, 07), Value = 0.70m },
                new Rate { CurrencySymbol = "JPY", Date = new DateTime(2023, 04, 07), Value = 107.0m },
            };

            var startDate = new DateTime(2023, 04, 01);
            var endDate = new DateTime(2023, 04, 07);
            var moneyUsd = 1000m;

            // Act
            var result = service.Calculate(rates, startDate, endDate, moneyUsd);

            // Assert
            Assert.Equal("JPY", result.Currency);
            Assert.Equal(new DateTime(2023, 04, 05), result.BuyDate);
            Assert.Equal(new DateTime(2023, 04, 07), result.SellDate);
            Assert.Equal(980.65m, result.Revenue, 2);
        }
    }
}
