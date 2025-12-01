using Application.Helpers;
using Application.Interfaces;
using Application.Services;
using Entities;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(HistoryProfile));
            services.AddScoped<IHistoryService, HistoryService>();
            return services;
        }
    }
}
