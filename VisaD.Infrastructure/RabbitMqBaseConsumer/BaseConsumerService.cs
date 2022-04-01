using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisaD.Infrastructure.RabbitMqBaseConsumer
{
    public abstract class BaseConsumerService<T> : BackgroundService
    {
        private readonly BaseConsumerConnectionService _consumerConnection;
        private readonly string _exchangeName;

        protected abstract Action<T> OnReceive { get; }
        protected IModel Channel { get; private set; }

        public BaseConsumerService(
            BaseConsumerConnectionService consumerConnection, 
            string exchangeName
        )
        {
            this._consumerConnection = consumerConnection;
            this._exchangeName = exchangeName;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Channel = _consumerConnection.Connection.CreateModel();

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += (obj, e) => {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                var model = JsonConvert.DeserializeObject<T>(body);
                try
                {
                    OnReceive.Invoke(model);
                    Channel.BasicAck(e.DeliveryTag, false);
                }
                catch (Exception)
                {
                    Channel.BasicReject(e.DeliveryTag, true);
                }
            };

            Channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);

            var queue = Channel.QueueDeclare();
            Channel.QueueBind(queue.QueueName, _exchangeName, "");
            Channel.BasicConsume(queue: queue.QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            base.Dispose();

            return base.StopAsync(cancellationToken);
        }
    }
}
