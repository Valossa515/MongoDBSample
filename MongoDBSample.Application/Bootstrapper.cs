using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MongoDBSample.Application
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddApplicationValidators(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}