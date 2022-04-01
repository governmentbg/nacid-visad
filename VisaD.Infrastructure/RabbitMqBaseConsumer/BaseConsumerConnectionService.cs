using RabbitMQ.Client;
using System;
using System.Security.Authentication;
using VisaD.Infrastructure.RabbitMqBaseConsumer.Configurations;

namespace VisaD.Infrastructure.RabbitMqBaseConsumer
{
    public abstract class BaseConsumerConnectionService
    {
        public IConnection Connection { get; private set; }

        public BaseConsumerConnectionService(IConsumerConfiguration configuration)
        {
            var factory = new ConnectionFactory {
                HostName = configuration.Host,
                Port = configuration.Port,
                UserName = configuration.Username,
                Password = configuration.Password,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(configuration.HeartbeatTimeout),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(configuration.NetworkRecoveryInterval),
                DispatchConsumersAsync = false,
                Ssl = new SslOption {
                    Enabled = configuration.SslEnabled,
                    ServerName = configuration.SslServerName,
                    CertPath = configuration.SslCertPath,
                    CertPassphrase = configuration.SslCertPassphrase,
                    Version = SslProtocols.Tls12
                }
            };

            Connection = factory.CreateConnection();
        }
    }
}
