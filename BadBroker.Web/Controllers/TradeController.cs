using BadBroker.Business.Models;
using BadBroker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadBroker.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TradeController : ControllerBase
    {
        private readonly IRateLoaderService _rateLoaderService;
        private readonly ICurrencyTradeService _tradeService;

        public TradeController(IRateLoaderService rateLoaderService, ICurrencyTradeService tradeService)
        {
            _rateLoaderService = rateLoaderService;
            _tradeService = tradeService;
        }

        /// <summary>
        /// Get the best deal
        /// </summary>
        /// <param name="beginning of period"></param>
        /// <param name="end of period"></param>
        /// <param name="money Usd"></param>
        /// <returns>Best trade</returns>
        [HttpPost]
        public async Task<CurrencyTrade> GetBestTrade([FromBody] RateQueryModel rateQuery)
        {
            var rates = await _rateLoaderService.GetRates(rateQuery.StartDate, rateQuery.EndDate);
            var response = _tradeService.Calculate(rates, rateQuery.StartDate, rateQuery.EndDate, rateQuery.MoneyUsd);
            return response;
        }

    }
}
