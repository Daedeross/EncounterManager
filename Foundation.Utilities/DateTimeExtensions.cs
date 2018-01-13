namespace Foundation.Utilities
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;

            if (target <= start)
            {
                target += 7;
            }

            return from.Date.AddDays(target - start);
        }

        public static double ToMonths(this DateTime date)
        {
            return date.Year * 12.0
                   + date.Month
                   + date.Day / (double)CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(date.Year, date.Month);
        }
    }
}
