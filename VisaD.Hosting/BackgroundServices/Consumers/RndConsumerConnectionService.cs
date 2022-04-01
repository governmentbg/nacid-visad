using Microsoft.Extensions.Options;
using VisaD.Hosting.BackgroundServices.Configurations;
using VisaD.Infrastructure.RabbitMqBaseConsumer;

namespace VisaD.Hosting.BackgroundServices.Consumers
{
	public class RndConsumerConnectionService : BaseConsumerConnectionService
    {
        public RndConsumerConnectionService(IOptions<RndConsumerConfiguration> configuration)
            : base(configuration.Value)
        {

        }
    }
}
