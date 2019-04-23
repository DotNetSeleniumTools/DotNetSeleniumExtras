using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.PageObjects.Tests;

namespace SeleniumExtras.MemberBuilders
{
    public class WrappedElementBuilder : IMemberBuilder
    {
        public bool CreateObject(Type memberType, IElementLocator locator, IEnumerable<By> bys, bool cache, out object createdObject)
        {
            createdObject = null;

            if (typeof(IWrapsElement).IsAssignableFrom(memberType))
            {
                var webElement = WebElementProxy.CreateProxy(locator, bys, cache);
                createdObject = WrapsElementFactory.Wrap(memberType, webElement);
                return true;
            }

            return false;
        }
    }
}
