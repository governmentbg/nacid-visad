using VisaD.Infrastructure.RabbitMqBaseConsumer.Configurations;

namespace VisaD.Hosting.BackgroundServices.Configurations
{
	public class RndConsumerConfiguration : IConsumerConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string RndOrganizationUpdateExchange { get; set; }
        public string RndSpecialityUpdateExchange { get; set; }
        public int HeartbeatTimeout { get; set; }
        public int NetworkRecoveryInterval { get; set; }
        public bool IsConsumerEnabled { get; set; }
        public bool SslEnabled { get; set; }
        public string SslServerName { get; set; }
        public string SslCertPath { get; set; }
        public string SslCertPassphrase { get; set; }
    }
}
