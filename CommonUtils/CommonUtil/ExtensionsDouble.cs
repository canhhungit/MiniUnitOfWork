using System;

namespace CommonUtil
{
    public static class ExtensionsDouble
    {
        /// <summary>
        /// Divides tow numbers considering a tolerance.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="number2"></param>
        /// <returns></returns>
        public static double DivideBy(this double number, double number2) => number2 <= Utils.Tolerance
                ? double.NaN
                : number / number2;
        /// <summary>
        /// Transforms a radians number to degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(this double radians) => radians * 180.0 / Math.PI;
        /// <summary>
        /// Transforms a degrees number to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double ToRadians(this double degrees) => degrees * Math.PI / 180.0;
    }
}
