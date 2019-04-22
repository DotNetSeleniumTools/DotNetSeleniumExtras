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
using OpenQA.Selenium;
using System.Reflection;

#if !NETSTANDARD2_0
using System;
using System.Runtime.Remoting.Messaging;
#else
using System.Reflection;
#endif

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Represents a proxy class for a list of elements to be used with the PageFactory.
    /// </summary>
    public class WebElementListProxy : WebDriverObjectProxy
    {
        private List<IWebElement> collection = null;

#if !NETSTANDARD2_0
        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementListProxy"/> class.
        /// </summary>
        /// <param name="typeToBeProxied">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementListProxy(Type typeToBeProxied, IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(typeToBeProxied, locator, bys, cache)
        {
        }
#endif

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
        public static object CreateProxy(IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
        {
#if !NETSTANDARD2_0
            return new WebElementListProxy(typeof(IList<IWebElement>), locator, bys, cacheLookups).GetTransparentProxy();
#else
            var proxy = Create<IList<IWebElement>, WebElementListProxy>();
            ((WebElementListProxy)(object)proxy).SetSearchProperites(locator, bys, cacheLookups);
            return proxy;
#endif
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Invokes the method that is specified in the provided <see cref="IMessage"/> on the
        /// object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/> that contains an <see cref="IDictionary"/>  of
        /// information about the method call. </param>
        /// <returns>The message returned by the invoked method, containing the return value and any
        /// out or ref parameters.</returns>
        public override IMessage Invoke(IMessage msg)
        {
            var elements = this.ElementList;

            try
            {
                return WebDriverObjectProxy.InvokeMethod(msg as IMethodCallMessage, elements);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }
#else
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
#endif
    }
}
