using BadBroker.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadBroker.Business.Services.Interfaces
{
    public interface IRateLoaderService
    {
        public Task<List<Rate>> GetRates(DateTime startDate, DateTime endDate);
    }
}
