using Microsoft.Extensions.DependencyInjection;
using VisaD.Hosting.BackgroundServices.Consumers;

namespace VisaD.Hosting.BackgroundServices
{
	public static class DependencyInjection
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services
                .AddSingleton<RndConsumerConnectionService>()
                .AddHostedService<InstitutionConsumer>()
                .AddHostedService<SpecialityConsumer>()
            ;

            return services;
        }
    }
}
