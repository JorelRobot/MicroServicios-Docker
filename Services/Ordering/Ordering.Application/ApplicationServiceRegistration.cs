using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            /// Injection goes here
            /// 

            /// Configuramos AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            /// Configuramos Fluent Application
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            /// Configuramos Mediatr
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
