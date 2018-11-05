#if NETSTANDARD2_0

using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumExtras.PageObjects;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Due to Linq optimized execution in dotnet core for IList, some methods lead to multiple elements retrieval.
    /// In this class IList is wrapped in IEnumerable to disable that 'optimized' evaluation.
    /// </summary>
    public static class WebElementEnumerable
    {
        public static IEnumerable<TResult> Select<TElement, TResult>(this IList<TElement> webElements, Func<TElement, TResult> selector)
            => webElements.ToEnumerable().Select(selector);

        public static IEnumerable<TElement> Where<TElement>(this IList<TElement> webElements, Func<TElement, bool> selector)
            => webElements.ToEnumerable().Where(selector);

        public static List<TElement> ToList<TElement>(this IList<TElement> webElements)
            => webElements.ToEnumerable().ToList();

        public static TElement[] ToArray<TElement>(this IList<TElement> webElements)
            => webElements.ToEnumerable().ToArray();

        private static IEnumerable<T> ToEnumerable<T>(this IList<T> enumerable)
        {
            IEnumerable<T> ToEnumerableInner(IList<T> e)
            {
                foreach (var element in e)
                    yield return element;
            }

            return enumerable is WebElementListProxy
                ? ToEnumerableInner(enumerable)
                : enumerable;
        }
    }
}

#endif