using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime toUnspecified(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        }

        public static DateTime nowUnspecified()
        {
            return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        }
    }
}
