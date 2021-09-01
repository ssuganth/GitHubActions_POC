using System;

namespace SampleApp.Utility.Interfaces
{
    public interface IDatetimeHelper
    {
        DateTime Now();
        DateTime UtcNow();
        DateTime? ToDateTime(string date);
    }
}