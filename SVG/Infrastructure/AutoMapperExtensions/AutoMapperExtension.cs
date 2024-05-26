using AutoMapper;

namespace SVG.API.Infrastructure.AutoMapperExtensions
{
    internal static class AutoMapperExtension
    {
        internal static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(mc =>
            {
                mc.AddProfile<DataProfile>();
            });

            services.AddSingleton(sp => configuration.CreateMapper());
            return services;
        }
    }
}
