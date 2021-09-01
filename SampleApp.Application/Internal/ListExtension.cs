using System.Collections.Generic;
using System.Linq;

namespace SampleApp.Application.Internal
{
    public static class ListExtension
    {
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}