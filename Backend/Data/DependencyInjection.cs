using Data.Data;
using Data.Managers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {

            services.AddScoped<IHistoryManager, HistoryManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Не задана строка подключения");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly("History"));
            });

            return services;
        }
    }
}
