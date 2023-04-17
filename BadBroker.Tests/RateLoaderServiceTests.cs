using BadBroker.Business.Services.Implementations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BadBroker.Tests
{
    public class RateLoaderServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLoaderService> _logger;

        public RateLoaderServiceTests()
        {
            _httpClient = new HttpClient();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new LoggerFactory().CreateLogger<RateLoaderService>();
        }

        [Fact]
        public async Task GetRates_ReturnsValidRates()
        {
            // Arrange
            var rateLoaderService = new RateLoaderService(_httpClient, _cache, _logger);
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 31);

            // Act
            var rates = await rateLoaderService.GetRates(startDate, endDate);

            // Assert
            Assert.NotEmpty(rates);
            Assert.All(rates, rate => Assert.NotNull(rate.CurrencySymbol));
            Assert.All(rates, rate => Assert.NotEqual(decimal.Zero, rate.Value));
            Assert.All(rates, rate => Assert.InRange(rate.Date, startDate, endDate));
        }

        [Fact]
        public async Task GetRates_ThrowsExceptionOnFailedResponse()
        {
            // Arrange
            var httpClient = new HttpClient(new MockHttpMessageHandler());
            var service = new RateLoaderService(httpClient, _cache, _logger);
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 31);

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetRates(startDate, endDate));
        }

        [Fact]
        public async Task GetRates_ReturnsCachedRates()
        {
            // Arrange
            var rateLoaderService = new RateLoaderService(_httpClient, _cache, _logger);
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 31);

            // Act
            var rates1 = await rateLoaderService.GetRates(startDate, endDate);
            var rates2 = await rateLoaderService.GetRates(startDate, endDate);

            // Assert
            Assert.Equal(rates1, rates2);
        }
    }
}
