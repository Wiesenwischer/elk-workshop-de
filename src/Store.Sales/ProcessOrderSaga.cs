using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Commands;
using Store.Messages.Events;

namespace Store.Sales
{
    using Microsoft.Extensions.Logging;

    internal class ProcessOrderSaga :
        Saga<ProcessOrderSaga.OrderData>,
        IAmStartedByMessages<SubmitOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
    {
        readonly ILogger<ProcessOrderSaga> log;

        public ProcessOrderSaga(ILogger<ProcessOrderSaga> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Data.OrderNumber = message.OrderNumber;
            Data.ProductIds = message.ProductIds;
            Data.ClientId = message.ClientId;

            log.LogInformation("Starting cool down period for Order #{OrderNumber}.", message.OrderNumber);
            return RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        }

        public Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            MarkAsComplete();
            log.LogTrace("MarkAsComplete called for Order #{OrderNumber}.", Data.OrderNumber);

            log.LogInformation("Cooling down period for Order #{OrderNumber} has elapsed.", Data.OrderNumber);

            var orderAccepted = new OrderAccepted
            {
                OrderNumber = Data.OrderNumber,
                ProductIds = Data.ProductIds,
                ClientId = Data.ClientId
            };
            log.LogTrace("Publishing: {@Event}", orderAccepted);
            return context.Publish(orderAccepted);
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            log.LogInformation("Order #{OrderNumber} was cancelled.", message.OrderNumber);

            MarkAsComplete();
            log.LogTrace("MarkAsComplete called for Order #{OrderNumber}.", message.OrderNumber);

            var orderCancelled = new OrderCancelled
            {
                OrderNumber = message.OrderNumber,
                ClientId = message.ClientId
            };
            log.LogTrace("Publishing: {@Event}", orderCancelled);
            return context.Publish(orderCancelled);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
        {
            mapper.ConfigureMapping<SubmitOrder>(message => message.OrderNumber)
                .ToSaga(sagaData => sagaData.OrderNumber);
            mapper.ConfigureMapping<CancelOrder>(message => message.OrderNumber)
                .ToSaga(sagaData => sagaData.OrderNumber);
        }

        public class OrderData :
            ContainSagaData
        {
            public int OrderNumber { get; set; }
            public string[] ProductIds { get; set; }
            public string ClientId { get; set; }
        }

        public class BuyersRemorseIsOver
        {
        }
    }
}