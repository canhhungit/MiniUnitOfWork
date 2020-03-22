using System;

namespace CommonUtil {
    public static class ExtensionsTimeSpan {
        /// <summary>
        /// Multiplies the TimeSpan by a factor.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static TimeSpan MultiplyBy ( this TimeSpan span, double factor ) => new TimeSpan ( ( long ) ( span.Ticks * factor ) );
        /// <summary>
        /// Divides the TimeSpan by a factor. Uses the MultiplyBy extension.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static TimeSpan DivideBy ( this TimeSpan span, double factor ) => span.MultiplyBy ( 1 / factor );
        /// <summary>
        /// Divide to TimeSpans and obtains the ration in double precision.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="span2"></param>
        /// <returns></returns>
        public static double DivideBy ( this TimeSpan span, TimeSpan span2 ) => ( double ) span.Ticks / span2.Ticks;
    }
}
