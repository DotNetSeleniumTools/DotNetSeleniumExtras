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
using System.Linq;
using OpenQA.Selenium;

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Represents a proxy class for a list of elements to be used with the PageFactory.
    /// </summary>
    internal class WrapsElementListProxy<T> : WebDriverObjectProxy, IList<T> where T : IWrapsElement
    {
        private IList<T>? _items;

        public WrapsElementListProxy(IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(locator, bys, cache)
        {
        }

        private IList<T> Items
        {
            get
            {
                // Find elements, and wrap them in IWrapsElement instances.
                // If caching enabled - use previously found elements, if any.
                if (_items == null || !Cache)
                {
                    _items = Locator
                        .LocateElements(Bys)
                        .Select(WrapsElementFactory.Wrap<T>)
                        .ToList();
                }
                return _items;
            }
        }

        #region Forwarded Items calls

        public T this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        public int Count => Items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        public void Add(T item) => Items.Add(item);

        public void Clear() => Items.Clear();

        public bool Contains(T item) => Items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        public int IndexOf(T item) => Items.IndexOf(item);

        public void Insert(int index, T item) => Items.Insert(index, item);

        public bool Remove(T item) => Items.Remove(item);

        public void RemoveAt(int index) => Items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        #endregion
    }
}
