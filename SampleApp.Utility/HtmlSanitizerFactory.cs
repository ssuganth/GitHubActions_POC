using Ganss.XSS;
using SampleApp.Utility.Interfaces;

namespace SampleApp.Utility
{
    public class HtmlSanitizerFactory : IHtmlSanitizerFactory
    {
        public HtmlSanitizer CreateHtmlSanitizer()
        {
            return new HtmlSanitizer();
        }
    }
}