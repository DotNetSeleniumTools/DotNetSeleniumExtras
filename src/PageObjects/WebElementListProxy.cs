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

using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Represents a proxy class for a list of elements to be used with the PageFactory.
    /// </summary>
    internal class WebElementListProxy : WebDriverObjectProxy,IList<IWebElement>
    {
        private List<IWebElement> collection = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementListProxy"/> class.
        /// </summary>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementListProxy(IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(locator, bys, cache)
        {
        }

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
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the
        /// element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the
        /// list of <see cref="IWebElement"/> objects.</returns>
        public static object CreateProxy(IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
        {
            return new WebElementListProxy(locator, bys, cacheLookups);
        }

        public IEnumerator<IWebElement> GetEnumerator()
        {
            return ElementList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) ElementList).GetEnumerator();
        }

        public void Add(IWebElement item)
        {
            ElementList.Add(item);
        }

        public void Clear()
        {
            ElementList.Clear();
        }

        public bool Contains(IWebElement item)
        {
            return ElementList.Contains(item);
        }

        public void CopyTo(IWebElement[] array, int arrayIndex)
        {
            ElementList.CopyTo(array, arrayIndex);
        }

        public bool Remove(IWebElement item)
        {
            return ElementList.Remove(item);
        }

        public int Count
        {
            get { return ElementList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<IWebElement>) ElementList).IsReadOnly; }
        }

        public int IndexOf(IWebElement item)
        {
            return ElementList.IndexOf(item);
        }

        public void Insert(int index, IWebElement item)
        {
            ElementList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ElementList.RemoveAt(index);
        }

        public IWebElement this[int index]
        {
            get { return ElementList[index]; }
            set { ElementList[index] = value; }
        }
    }
}