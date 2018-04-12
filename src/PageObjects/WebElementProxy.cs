// <copyright file="WebElementProxy.cs" company="WebDriver Committers">
// Licensed to the Software Freedom Conservancy (SFC) under one
// or more contributor license agreements. See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership. The SFC licenses this file
// to you under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Intercepts the request to a single <see cref="IWebElement"/>
    /// </summary>
    internal sealed class WebElementProxy : WebDriverObjectProxy, IWrapsElement, IWebElement
    {
        private IWebElement cachedElement;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementProxy"/> class.
        /// </summary>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that determines
        /// how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementProxy(IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(locator, bys, cache)
        {
        }

        /// <summary>
        /// Gets the <see cref="IWebElement"/> wrapped by this object.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return this.Element; }
        }

        /// <summary>
        /// Gets the IWebElement object this proxy represents, returning a cached one if requested.
        /// </summary>
        private IWebElement Element
        {
            get
            {
                if (!this.Cache || this.cachedElement == null)
                {
                    this.cachedElement = this.Locator.LocateElement(this.Bys);
                }

                return this.cachedElement;
            }
        }

        /// <summary>
        /// Creates an object used to proxy calls to properties and methods of an <see cref="IWebElement"/> object.
        /// </summary>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the list of <see cref="IWebElement"/> objects.</returns>
        public static object CreateProxy(IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
        {
            return new WebElementProxy(locator, bys, cacheLookups);
        }

        public IWebElement FindElement(By @by)
        {
            return Element.FindElement(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return Element.FindElements(@by);
        }

        public void Clear()
        {
            Element.Clear();
        }

        public void SendKeys(string text)
        {
            Element.SendKeys(text);
        }

        public void Submit()
        {
            Element.Submit();
        }

        public void Click()
        {
            Element.Click();
        }

        public string GetAttribute(string attributeName)
        {
            return Element.GetAttribute(attributeName);
        }

        public string GetProperty(string propertyName)
        {
            return Element.GetProperty(propertyName);
        }

        public string GetCssValue(string propertyName)
        {
            return Element.GetCssValue(propertyName);
        }

        public string TagName
        {
            get { return Element.TagName; }
        }

        public string Text
        {
            get { return Element.Text; }
        }

        public bool Enabled
        {
            get { return Element.Enabled; }
        }

        public bool Selected
        {
            get { return Element.Selected; }
        }

        public Point Location
        {
            get { return Element.Location; }
        }

        public Size Size
        {
            get { return Element.Size; }
        }

        public bool Displayed
        {
            get { return Element.Displayed; }
        }
    }
}