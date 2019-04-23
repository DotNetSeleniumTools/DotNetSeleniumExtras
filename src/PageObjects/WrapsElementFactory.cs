using System;
using OpenQA.Selenium;

namespace SeleniumExtras
{
    internal static class WrapsElementFactory
    {
        public static T Wrap<T>(IWebElement webElement) where T : IWrapsElement
        {
            return (T)Wrap(typeof(T), webElement);
        }

        public static object Wrap(Type wrapsElementType, IWebElement webElement)
        {
            var iWebElementConstructor = wrapsElementType.GetConstructor(new[] { typeof(IWebElement) });

            // Option 1 - T has constructor with IWebElement parameter
            if (iWebElementConstructor != null)
            {
                return iWebElementConstructor.Invoke(new object[] { webElement });
            }

            // Option 2 - T has parameterless constructor, and setter on WrappedElement property
            var parameterlessConstructor = wrapsElementType.GetConstructor(Array.Empty<Type>());
            var wrappedElementProperty = wrapsElementType.GetProperty(nameof(IWrapsElement.WrappedElement));

            if (parameterlessConstructor != null && wrappedElementProperty?.CanWrite == true)
            {
                var wrappedInstance = parameterlessConstructor.Invoke(Array.Empty<object>());
                wrappedElementProperty.SetValue(wrappedInstance, webElement);
                return wrappedInstance;
            }

            throw new NotSupportedException($"Cannot create instance of type '{wrapsElementType.FullName}'");
        }
    }
}
