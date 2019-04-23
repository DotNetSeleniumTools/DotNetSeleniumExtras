// <copyright file="WebElementListProxy.cs" company="WebDriver Committers">
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
using System.Reflection;
using OpenQA.Selenium;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Represents a proxy class for a list of elements to be used with the PageFactory.
    /// </summary>
    public class WebElementListProxy : WebDriverObjectProxy
    {
        private List<IWebElement> collection = null;

        /// <summary>
        /// Gets the list of IWebElement objects this proxy represents, returning a cached one if requested.
        /// </summary>
        private List<IWebElement> ElementList
        {
            get
            {
                if (!this.Cache || this.collection == null)
                {
                    this.collection = new List<IWebElement>();
                    this.collection.AddRange(this.Locator.LocateElements(this.Bys));
                }

                return this.collection;
            }
        }

        /// <summary>
        /// Creates an object used to proxy calls to properties and methods of the
        /// list of <see cref="IWebElement"/> objects.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the
        /// element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the
        /// list of <see cref="IWebElement"/> objects.</returns>
        public static IList<IWebElement> CreateProxy(IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
        {
            var proxy = Create<IList<IWebElement>, WebElementListProxy>();
            ((WebElementListProxy)proxy).SetSearchProperites(locator, bys, cacheLookups);
            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                return targetMethod.Invoke(ElementList, args);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }
    }
}
