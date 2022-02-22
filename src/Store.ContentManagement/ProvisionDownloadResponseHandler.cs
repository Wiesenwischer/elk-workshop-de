using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

namespace Store.ContentManagement
{
    using System;
    using Microsoft.Extensions.Logging;

    public class ProvisionDownloadResponseHandler :
        IHandleMessages<ProvisionDownloadResponse>
    {
        readonly ILogger<ProvisionDownloadResponseHandler> log;

        public ProvisionDownloadResponseHandler(ILogger<ProvisionDownloadResponseHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        Dictionary<string, string> productIdToUrlMap = new Dictionary<string, string>
        {
            { "videos", "https://particular.net/videos-and-presentations" },
            { "training", "https://particular.net/onsite-training" },
            { "documentation", "https://docs.particular.net/" },
            { "customers", "https://particular.net/customers" },
            { "platform", "https://particular.net/service-platform" },
        };

        public Task Handle(ProvisionDownloadResponse message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            log.LogInformation("Download for Order #{OrderNumber} has been provisioned, publishing Download Ready event.", message.OrderNumber);

            log.LogInformation("Download for Order #{OrderNumber} is ready, publishing it.", message.OrderNumber);
            var downloadIsReady = new DownloadIsReady
            {
                OrderNumber = message.OrderNumber,
                ClientId = message.ClientId,
                ProductUrls = new Dictionary<string, string>()
            };

            foreach (string productId in message.ProductIds)
            {
                downloadIsReady.ProductUrls.Add(productId, productIdToUrlMap[productId]);
            }
            log.LogTrace("Download for Order #{OrderNumber} is ready, publishing DownloadIsReady: {@response}", message.OrderNumber, @downloadIsReady);

            return context.Publish(downloadIsReady);
        }
    }
}