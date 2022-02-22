using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Commands;
using Store.Messages.Events;
using Microsoft.Extensions.Logging;

namespace Store.Sales
{
    using System;

    internal class SubmitOrderHandler :
        IHandleMessages<SubmitOrder>
    {
        readonly ILogger<SubmitOrderHandler> log;

        public SubmitOrderHandler(ILogger<SubmitOrderHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            string products = string.Join(", ", message.ProductIds);
            log.LogInformation("Received an Order #{OrderNumber} for [{ProductIds}] product(s).", message.OrderNumber, products);

            log.LogInformation("The credit card values will be encrypted when looking at the messages in the queues");
            log.LogInformation("CreditCard Number is {CreditCardNumber}", message.CreditCardNumber);
            log.LogInformation("CreditCard Expiration Date is {ExpirationDate}", message.ExpirationDate);

            // tell the client the order was received
            var orderPlaced = new OrderPlaced
            {
                ClientId = message.ClientId,
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds
            };
            log.LogTrace("Publishing: {@Event}", orderPlaced);
            return context.Publish(orderPlaced);
        }
    }
}
