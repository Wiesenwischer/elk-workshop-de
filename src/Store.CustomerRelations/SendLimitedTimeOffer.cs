using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;

namespace Store.CustomerRelations
{
    using System;
    using Microsoft.Extensions.Logging;

    internal class SendLimitedTimeOffer :
        IHandleMessages<ClientBecamePreferred>
    {
        readonly ILogger<SendLimitedTimeOffer> log;

        public SendLimitedTimeOffer(ILogger<SendLimitedTimeOffer> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            log.LogInformation("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {ClientId}", message.ClientId);
            return Task.CompletedTask;
        }
    }
}