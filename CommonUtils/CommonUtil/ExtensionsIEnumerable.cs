using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtil
{
    public static class ExtensionsIEnumerable
    {
        // doubles
        /// <summary>
        /// Computes the deviation of a set of numbers.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Deviation(this IEnumerable<double> list) => Math.Sqrt(list.Variance());
        /// <summary>
        /// Computes the deviation of a set of objects based on a numerical transformation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double Deviation<T>(this IEnumerable<T> list, Func<T, double> f) => Math.Sqrt(list.Variance(f));
        /// <summary>
        /// Computes the variance of a set of objects based on a numerical transformation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double Variance<T>(this IEnumerable<T> list, Func<T, double> f)
        {
            List<double> doubles = list
                .Select(f.Invoke)
                .ToList();
            double average = doubles.Average();
            double variance = doubles
                .Sum(d => Math.Pow(d - average, 2));
            return variance;
        }
        /// <summary>
        /// Computes the variance of a set of numbers.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<double> list)
        {
            List<double> doubles = list.ToList();
            double average = doubles.Average();
            double variance = doubles
                .Sum(d => Math.Pow(d - average, 2));
            return variance;
        }
        /// <summary>
        /// Computes the weighted average of a set of objects based on a numerical transformation and a 
        /// weight function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="function"></param>
        /// <param name="weightFunction"></param>
        /// <returns></returns>
        public static double WeightedAverage<T>(this IEnumerable<T> list
            , Func<T, int, double> function
            , Func<T, int, double> weightFunction)
        {
            double sum = 0;
            double sumWeights = 0;
            list
                .ForEach((element, index) =>
                {
                    sum += function(element, index) * weightFunction(element, index);
                    sumWeights += weightFunction(element, index);
                });
            return sum.DivideBy(sumWeights);
        }
        /// <summary>
        /// Computes the average of a set of objects based on a numerical transformation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double Average<T>(this IEnumerable<T> list, Func<T, int, double> f) =>
            list.WeightedAverage(f, (element, index) => 1);
        /// <summary>
        /// Computes the exponential averages of a set of objects based on a numerical transformation.
        /// It uses the WeightedAverage extension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double ExponentialAverage<T>(this IEnumerable<T> list, Func<T, int, double> f)
        {
            T[] array = list
                .ToArray();

            return array
                .WeightedAverage(f, (element, index) => Math.Exp(index - array.Length));
        }
        /// <summary>
        /// Checks if a set of object is sorted based on a numerical transformation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool IsSortedBy<T>(this IEnumerable<T> list, Func<T, double> f) => list
                .Select(f)
                .IsSorted();
        /// <summary>
        /// Checks if a set of numbers is sorted.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsSorted(this IEnumerable<double> list)
        {
            bool sorted = true;
            List<double> doubles = list
                .ToList();
            for (int index = 0; index < doubles.Count - 1; index++)
            {
                double previousValue = doubles[index];
                double nextValue = doubles[index + 1];
                sorted = nextValue >= previousValue;
                if (!sorted)
                {
                    break;
                }
            }

            return sorted;
        }
        /// <summary>
        /// Checks if a set of numbers is sorted descendantly.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsDescendantlySorted(this IEnumerable<double> list) => list
            .Reverse()
            .IsSorted();
        /// <summary>
        /// Checks if a set of objects is sorted descendantly based on a numerical transformation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool IsDescendantlySortedBy<T>(this IEnumerable<T> list, Func<T, double> f) => list
                .Select(f)
                .IsDescendantlySorted();
        /// <summary>
        /// Finds the set of pairs of a set of objects that are most distant from each other based on a distance function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static IEnumerable<(T element1, T element2)> MaxDistantElementsBy<T>(this IEnumerable<T> list, Func<T, T, double> distance)
        {
            List<T> elements = list.ToList();
            IExtremaEnumerable<(T element, T element2)> result = elements
                .SelectMany((element, index) => elements
                  .Select(element2 => (element, element2)))
                .MaxBy(pair => distance(pair.element, pair.element2));
            return result;
        }
        /// <summary>
        /// Finds the maximum distance between elements of a set of objects based on a distance function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static double MaxDistanceBy<T>(this IEnumerable<T> list, Func<T, T, double> distance)
        {
            List<T> elements = list.ToList();
            double result = elements
                .SelectMany((element, index) => elements
                  .Select(element2 => (element, element2))
                )
                .Max(pair => distance(pair.element, pair.element2));
            return result;
        }
    }
}