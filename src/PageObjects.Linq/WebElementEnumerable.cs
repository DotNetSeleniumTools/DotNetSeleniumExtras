using System.Collections.Generic;

namespace System.Linq
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
            foreach (var element in enumerable)
                yield return element;
        }
    }
}