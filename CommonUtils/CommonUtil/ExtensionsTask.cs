using System;
using System.Threading.Tasks;

namespace CommonUtil
{
    public static class ExtensionsTask
    {
        /// <summary>
        /// Maps the result with a function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> fn) => fn(await task);
        /// <summary>
        /// Maps the result of a task with another task based on a function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static async Task<TResult> FlatMap<T, TResult>(this Task<T> task, Func<T, Task<TResult>> fn) => await fn(await task);
        /// <summary>
        /// Executes an action foreach Task provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static async Task ForEach<T>(this Task<T> task, Action<T> fn) => fn(await task);
    }
}
