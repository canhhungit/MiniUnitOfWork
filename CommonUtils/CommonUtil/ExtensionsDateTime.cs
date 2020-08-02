using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtil
{
    public static class ExtensionsDateTime
    {
        /// <summary>
        /// Checks if a date is February 29th.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsFebruary29S(this DateTime date) => date.Day == 29 &&
                   date.Month == 2;
        /// <summary>
        /// Returns the days between two dates.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDaysTo(this DateTime date, DateTime lastDay) => GetDatesTo(date, lastDay, new TimeSpan(1, 0, 0, 0));
        /// <summary>
        /// Returns the dates between two dates at span intervals.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lastDate"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDatesTo(this DateTime date, DateTime lastDate, TimeSpan span)
        {
            DateTime currentDate = date;
            yield return currentDate;

            int direction = date <= lastDate ? 1 : -1;
            TimeSpan increment = span.MultiplyBy(direction);
            bool IsLastDateReached(DateTime current)
            {
                return direction > 0 ? current > lastDate : current < lastDate;
            }

            currentDate += increment;
            bool lastDateReached = IsLastDateReached(currentDate);
            while (!lastDateReached)
            {
                yield return currentDate;

                currentDate += increment;
                lastDateReached = IsLastDateReached(currentDate);
            }
        }
        /// <summary>
        /// Computes the minimum date of a set of dates.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime Min(params DateTime[] days) => days.Min();
        /// <summary>
        /// Computes the maximum date of a set of dates.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime Max(params DateTime[] days) => days.Max();
        /// <summary>
        /// Add some days to a date avoiding the effect of leap years.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime AddDaysWithoutLeapYear(this DateTime input, int days)
        {
            DateTime output = input;

            if (days == 0)
            {
                return output;
            }

            int increment = days > 0
                ? 1
                : -1; //this will be used to increment or decrement the date.
            int daysAbs = Math.Abs(days); //get the absolute value of days to add
            int daysAdded = 0; // save the number of days added here
            while (daysAdded < daysAbs)
            {
                output = output.AddDays(increment);
                if (!(output.Month == 2 &&
                      output.Day == 29)) //don't increment the days added if it is a leap year day
                {
                    daysAdded++;
                }
            }

            return output;
        }
        /// <summary>
        /// Substract a TimeSpan to a DateTime and returns the result.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static DateTime Substract(this DateTime date, TimeSpan span) => date.Add(span.MultiplyBy(-1));
    }
}
