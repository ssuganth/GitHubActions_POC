using System;
using SampleApp.Utility.Interfaces;

namespace SampleApp.Utility
{
    public class HtmlSanitizerHelper : IHtmlSanitizerHelper
    {
        #region Members

        private readonly IHtmlSanitizerFactory _htmlSanitizerFactory;

        #endregion

        #region Ctor

        public HtmlSanitizerHelper(IHtmlSanitizerFactory htmlSanitizerFactory)
        {
            _htmlSanitizerFactory = htmlSanitizerFactory;
        }

        #endregion
        public string SanitizeInput(string input)
        {
            var sanitizer = _htmlSanitizerFactory.CreateHtmlSanitizer();
            var sanitizedInput = sanitizer.Sanitize(input);
            var isInputMalicious = string.IsNullOrEmpty(sanitizedInput);
            if(isInputMalicious) throw new ArgumentException(input);
            return sanitizer.Sanitize(input);
        }
    }
}