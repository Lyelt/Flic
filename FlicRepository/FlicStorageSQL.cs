using FlicCommon;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlicRepository
{
    public class FlicStorageSQL : IFlicHandler
    {
        private ILogger<FlicStorageSQL> _logger;

        public FlicStorageSQL(ILogger<FlicStorageSQL> logger)
        {
            _logger = logger;
        }

        public async Task HandleFlicRequestAsync(FlicRequest request)
        {
            _logger.LogInformation($"Storing {request}");
        }
    }
}
