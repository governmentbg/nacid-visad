using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Public.Hosting.Extensions
{
    public static class JsonConfigurationExtension
    {
        public static IMvcBuilder AddJson(this IMvcBuilder builder)
        {
            builder
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = JsonSerializerSettings.ReferenceLoopHandling;
                    options.SerializerSettings.NullValueHandling = JsonSerializerSettings.NullValueHandling;
                    options.SerializerSettings.DefaultValueHandling = JsonSerializerSettings.DefaultValueHandling;
                    options.SerializerSettings.DateFormatHandling = JsonSerializerSettings.DateFormatHandling;
                    options.SerializerSettings.ContractResolver = JsonSerializerSettings.ContractResolver;
                });
            JsonConvert.DefaultSettings = () => JsonSerializerSettings;

            return builder;
        }

        private static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
#if DEBUG
            Formatting = Formatting.Indented,
#else
                    Formatting = Formatting.None,
#endif
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}