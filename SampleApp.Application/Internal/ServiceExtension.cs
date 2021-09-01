using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Application.Common.Behaviors;
using SampleApp.Application.Common.Builders;
using SampleApp.Persistence.Internal;
using SampleApp.Utility.Internal;
using System.Reflection;

namespace SampleApp.Application.Internal
{
    public static class ServiceExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //Load First DbContext Service
            services.AddPersistence(configuration);

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidation<,>));
            services.AddSingleton<ICustomExceptionBuilder, CustomExceptionBuilder>();

            services.AddUtility();
        }
    }
}