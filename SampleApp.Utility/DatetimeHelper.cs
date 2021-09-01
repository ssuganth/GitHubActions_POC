using System;
using SampleApp.Utility.Interfaces;

namespace SampleApp.Utility
{
    public class DatetimeHelper : IDatetimeHelper
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime? ToDateTime(string date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}