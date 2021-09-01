using Ganss.XSS;

namespace SampleApp.Utility.Interfaces
{
    public interface IHtmlSanitizerFactory
    {
        HtmlSanitizer CreateHtmlSanitizer();
    }
}