using Microsoft.Extensions.DependencyInjection;
using SampleApp.Utility.Interfaces;

namespace SampleApp.Utility.Internal
{
    public static class ServiceExtension
    {
        public static void AddUtility(this IServiceCollection services)
        {
            services.AddSingleton<IDatetimeHelper, DatetimeHelper>();
            services.AddSingleton<IHtmlSanitizerFactory, HtmlSanitizerFactory>();
            services.AddSingleton<IHtmlSanitizerHelper, HtmlSanitizerHelper>();
        }
    }
}