using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

namespace Store.ContentManagement
{
    using System;
    using Microsoft.Extensions.Logging;

    public class OrderAcceptedHandler :
        IHandleMessages<OrderAccepted>
    {
        readonly ILogger<OrderAcceptedHandler> log;

        public OrderAcceptedHandler(ILogger<OrderAcceptedHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            log.LogInformation("Handling message with id {CorrelationId} - Order #{OrderNumber} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint", context.MessageId, message.OrderNumber);

            // send out a request (a event will be published when the response comes back)
            var request = new ProvisionDownloadRequest
            {
                ClientId = message.ClientId,
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds
            };
            log.LogTrace("Handling message with id {CorrelationId} - Order #{OrderNumber} has been accepted, Sending following ProvisionDownloadRequest: {@request}", context.MessageId, message.OrderNumber, request);
            return context.Send(request);
        }
    }
}