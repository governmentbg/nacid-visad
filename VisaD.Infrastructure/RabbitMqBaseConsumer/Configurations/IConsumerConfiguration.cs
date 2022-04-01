namespace VisaD.Infrastructure.RabbitMqBaseConsumer.Configurations
{
    public interface IConsumerConfiguration
    {
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string ExchangeName { get; set; }
        int HeartbeatTimeout { get; set; }
        int NetworkRecoveryInterval { get; set; }
        bool IsConsumerEnabled { get; set; }
        bool SslEnabled { get; set; }
        string SslServerName { get; set; }
        string SslCertPath { get; set; }
        string SslCertPassphrase { get; set; }

    }
}
