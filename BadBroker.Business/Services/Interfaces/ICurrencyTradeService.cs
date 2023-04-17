using BadBroker.Business.Models;
using System;
using System.Collections.Generic;

namespace BadBroker.Business.Services.Interfaces
{
    public interface ICurrencyTradeService
    {
        CurrencyTrade Calculate(IList<Rate> rates, DateTime startDate, DateTime endDate, decimal moneyUsd);
    }
}