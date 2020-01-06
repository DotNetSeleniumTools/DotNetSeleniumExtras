using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.PageObjects.Tests;

namespace SeleniumExtras.MemberBuilders
{
    internal class WrappedElementListBuilder : IMemberBuilder
    {
        public bool CreateObject(Type memberType, IElementLocator locator, IEnumerable<By> bys, bool cache, out object? createdObject)
        {
            createdObject = null;

            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                var elementType = memberType.GetGenericArguments()[0];

                if (typeof(IWrapsElement).IsAssignableFrom(elementType))
                {
                    var listType = typeof(WrapsElementListProxy<>).MakeGenericType(memberType.GetGenericArguments()[0]);
                    createdObject = Activator.CreateInstance(listType, new object[] { locator, bys, cache });
                    return true;
                }
            }

            return false;
        }
    }
}
