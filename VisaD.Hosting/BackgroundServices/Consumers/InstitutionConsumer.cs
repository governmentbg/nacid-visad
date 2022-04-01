using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using VisaD.Hosting.BackgroundServices.Configurations;
using VisaD.Application.InstitutionSpecialities.Dtos;
using VisaD.Application.InstitutionSpecialities.Services;
using VisaD.Infrastructure.RabbitMqBaseConsumer;

namespace VisaD.Hosting.BackgroundServices.Consumers
{
    public class InstitutionConsumer : BaseConsumerService<InstitutionDto>
    {
        private readonly IServiceProvider serviceProvider;

        public InstitutionConsumer(
            RndConsumerConnectionService consumer,
            IOptions<RndConsumerConfiguration> configuration,
            IServiceProvider serviceProvider
        )
            : base(consumer, configuration.Value.RndOrganizationUpdateExchange)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override Action<InstitutionDto> OnReceive => SaveInstitution;

        private async void SaveInstitution(InstitutionDto model)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var institutionSpecialitiesService = scope.ServiceProvider.GetRequiredService<InstitutionSpecialitiesService>();

                await institutionSpecialitiesService.SaveInstitution(model);
            }
        }
    }
}
