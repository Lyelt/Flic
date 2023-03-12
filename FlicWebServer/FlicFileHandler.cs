using FlicCommon;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicWebServer
{
    class FlicFileHandler : IFlicHandler
    {
        private ILogger<FlicFileHandler> _logger;

        public FlicFileHandler(ILogger<FlicFileHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleFlicRequestAsync(FlicRequest request)
        {
            _logger.LogInformation($"Storing {request}");
            using var w = File.AppendText(@"C:\Users\nicho\Desktop\flic.txt");
            await w.WriteLineAsync($"{request.Timestamp.ToLocalTime().DateTime.ToShortDateString()} at {request.Timestamp.ToLocalTime().DateTime.ToLongTimeString()}: Button {request.Id} {(request.Action == FlicAction.Hold ? "held" : request.Action == FlicAction.DoubleClick ? "double-clicked" : "clicked")}");
        }
    }
}
