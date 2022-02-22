using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.RequestResponse;

namespace Store.Operations
{
    using System;
    using Microsoft.Extensions.Logging;

    internal class ProvisionDownloadHandler :
        IHandleMessages<ProvisionDownloadRequest>
    {
        readonly ILogger<ProvisionDownloadHandler> log;

        public ProvisionDownloadHandler(ILogger<ProvisionDownloadHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(ProvisionDownloadRequest message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            string products = string.Join(", ", message.ProductIds);
            log.LogInformation("Provision the products and make the Urls available to the Content management for download ...[{ProductIds}] product(s) to provision", products);

            var response = new ProvisionDownloadResponse
            {
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds,
                ClientId = message.ClientId
            };
            log.LogTrace("Replying: {@Response}", response);
            return context.Reply(response);
        }
    }
}
