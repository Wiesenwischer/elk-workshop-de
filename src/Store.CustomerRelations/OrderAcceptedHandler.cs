using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;

namespace Store.CustomerRelations
{
    using Microsoft.Extensions.Logging;

    internal class OrderAcceptedHandler :
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

            log.LogInformation("Customer: {ClientId} is now a preferred customer publishing for other service concerns", message.ClientId);

            // publish this event as an asynchronous event
            var clientBecamePreferred = new ClientBecamePreferred
            {
                ClientId = message.ClientId,
                PreferredStatusExpiresOn = DateTime.Now.AddMonths(2)
            };
            log.LogTrace("Publishing: {@Event}", clientBecamePreferred);
            return context.Publish(clientBecamePreferred);
        }
    }
}