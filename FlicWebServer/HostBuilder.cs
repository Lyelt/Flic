using FlicCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlicWebServer
{
    public static class HostBuilder
    {
        private static IHost _host;

        public static async Task Init(CancellationToken ct) => await BuildHost().StartAsync(ct).ConfigureAwait(false);

        public static async Task Stop() => await (_host?.StopAsync() ?? Task.CompletedTask).ConfigureAwait(false);

        private static IHost BuildHost() =>
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions()
                        .AddLogging(l => l.ClearProviders().AddConsole())
                        .AddTransient<HttpListener>()
                        .AddTransient<IFlicHandler, FlicFileHandler>()
                        .AddHostedService<FlicListener>();
                })
                .Build();
    }
}
