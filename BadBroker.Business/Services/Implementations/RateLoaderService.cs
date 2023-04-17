using BadBroker.Business.Models;
using BadBroker.Business.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BadBroker.Business.Services.Implementations
{
    public class RateLoaderService : IRateLoaderService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLoaderService> _logger;

        public RateLoaderService(HttpClient httpClient, IMemoryCache cache, ILogger<RateLoaderService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Get rates from "exchangerate api"
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<Rate>> GetRates(DateTime startDate, DateTime endDate)
        {
            var cacheKey = $"ExchangeRate-{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}";

            var currencyRates = new List<Rate>();

            if (_cache.TryGetValue(cacheKey, out currencyRates))
            {
                _logger.LogInformation("loaded cached data");
                return currencyRates;
            }

            var uri = $"https://api.exchangerate.host/timeseries?start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}?base=USD&symbols=RUB,EUR,GBP,JPY";
            var response = await _httpClient.GetAsync(uri);
            _logger.LogInformation($"[{response.RequestMessage.Method}]({response.RequestMessage.RequestUri})");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to load rates. Status code: {response.StatusCode}");

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseData = JObject.Parse(responseJson);
            var rates = responseData["rates"].ToObject<Dictionary<string, Dictionary<string, decimal>>>();

            currencyRates = new List<Rate>();

            foreach (var rate in rates)
            {
                foreach (var dateRate in rate.Value)
                {
                    currencyRates.Add(new Rate
                    {
                        CurrencySymbol = dateRate.Key,
                        Value = dateRate.Value,
                        Date = DateTime.Parse(rate.Key)
                    });
                }
            }

            _logger.LogInformation("Data is cached.");
            _cache.Set(cacheKey, currencyRates, TimeSpan.FromMinutes(60));

            return currencyRates;
        }
    }
}
