using FlicCommon;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FlicWebServer
{
    public class FlicListener : IHostedService
    {
        private ILogger<FlicListener> _logger;
        private IFlicHandler _repository;
        private HttpListener _listener;

        private IPropagatorBlock<HttpListenerContext, FlicRequest> _listenerBlock;
        private ITargetBlock<FlicRequest> _handlerBlock;
        private Task _listenTask;

        private CancellationToken _ct;

        public FlicListener(ILogger<FlicListener> logger, IFlicHandler repository, HttpListener listener)
        {
            _logger = logger;
            _repository = repository;
            _listener = listener;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _ct = ct;
            _listenerBlock = new TransformBlock<HttpListenerContext, FlicRequest>(TransformHttpRequest, new ExecutionDataflowBlockOptions { CancellationToken = ct });
            _handlerBlock = new ActionBlock<FlicRequest>(async r => await HandleFlicRequestAsync(r), new ExecutionDataflowBlockOptions { CancellationToken = ct });
            _listenerBlock.LinkTo(_handlerBlock, new DataflowLinkOptions { PropagateCompletion = true });
            _listenTask = Task.Run(ListenAsync);
            return _listenTask;
        }

        public async Task StopAsync(CancellationToken ct)
        {
            _listenTask?.Wait();
            await _handlerBlock.Completion;
        }

        private async Task ListenAsync()
        {
            using (_ct.Register(() => _listener.Stop()))
            {
                var prefix = "http://+:8080/";
                _listener.Prefixes.Add(prefix);
                _listener.Start();

                _logger.LogInformation($"Listening on {prefix}");

                while (true)
                {
                    var context = await _listener.GetContextAsync();
                    await _listenerBlock.SendAsync(context);
                }
            }
        }

        private FlicRequest TransformHttpRequest(HttpListenerContext context)
        {
            var r = new FlicRequest(context.Request.Headers["flic-id"], FlicRequest.GetAction(context.Request.Headers["action"]), DateTimeOffset.UtcNow);
            _logger.LogInformation($"Received action {r.Action} from flic {r.Id} at {r.Timestamp.ToLocalTime()}");
            context.Response.StatusCode = 200;
            context.Response.Close();
            return r;
        }

        private async Task HandleFlicRequestAsync(FlicRequest request)
        {
            await _repository.HandleFlicRequestAsync(request);
        }
    }
}
