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
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Intercepts the request to a single <see cref="IWebElement"/>
    /// </summary>
    internal class WebElementProxy : WebDriverObjectProxy, IWrapsElement, IWebElement, ILocatable
    {
        private IWebElement? cachedElement;

        public WebElementProxy(IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(locator, bys, cache)
        {
        }

        /// <summary>
        /// Gets the IWebElement object this proxy represents, returning a cached one if requested.
        /// </summary>
        public IWebElement WrappedElement
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

        #region Forwarded WrappedElement calls

        public string TagName => WrappedElement.TagName;

        public string Text => WrappedElement.Text;

        public bool Enabled => WrappedElement.Enabled;

        public bool Selected => WrappedElement.Selected;

        public Point Location => WrappedElement.Location;

        public Size Size => WrappedElement.Size;

        public bool Displayed => WrappedElement.Displayed;

        public void Clear() => WrappedElement.Clear();

        public void Click() => WrappedElement.Click();

        public IWebElement FindElement(By by) => WrappedElement.FindElement(by);

        public ReadOnlyCollection<IWebElement> FindElements(By by) => WrappedElement.FindElements(by);

        public string GetAttribute(string attributeName) => WrappedElement.GetAttribute(attributeName);

        public string GetCssValue(string propertyName) => WrappedElement.GetCssValue(propertyName);

        public string GetProperty(string propertyName) => WrappedElement.GetProperty(propertyName);

        public void SendKeys(string text) => WrappedElement.SendKeys(text);

        public void Submit() => WrappedElement.Submit();

        public Point LocationOnScreenOnceScrolledIntoView
            => ((ILocatable)WrappedElement).LocationOnScreenOnceScrolledIntoView;

        public ICoordinates Coordinates
            => ((ILocatable)WrappedElement).Coordinates;

        public override int GetHashCode() => WrappedElement.GetHashCode();

        public override bool Equals(object? obj) => WrappedElement.Equals(obj);

        #endregion Forwarded WrappedElement calls
    }
}