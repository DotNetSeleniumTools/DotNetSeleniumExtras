using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.PageObjects.Tests;

namespace SeleniumExtras.MemberBuilders
{
    /// <summary>
    /// Creates member of <see cref="IList{IWebElement}"/> type
    /// </summary>
    internal class WebElementListBuilder : IMemberBuilder
    {
        public bool CreateObject(Type memberType, IElementLocator locator, IEnumerable<By> bys, bool cache, out object? createdObject)
        {
            createdObject = null;

            if (memberType == typeof(IList<IWebElement>))
            {
                createdObject = new WebElementListProxy(locator, bys, cache);
                return true;
            }

            return false;
        }
    }
}
