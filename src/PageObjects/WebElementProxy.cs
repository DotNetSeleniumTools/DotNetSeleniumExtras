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
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

#if !NETSTANDARD2_0
using System;
using System.Runtime.Remoting.Messaging;
#else
#endif

namespace SeleniumExtras.PageObjects
{
    /// <summary>
    /// Intercepts the request to a single <see cref="IWebElement"/>
    /// </summary>
    public class WebElementProxy : WebDriverObjectProxy, IWrapsElement
    {
        private IWebElement cachedElement;

#if !NETSTANDARD2_0
        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementProxy"/> class.
        /// </summary>
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that determines
        /// how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cache"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        private WebElementProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, bool cache)
            : base(classToProxy, locator, bys, cache)
        {
        }
#endif

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
        /// <param name="classToProxy">The <see cref="Type"/> of object for which to create a proxy.</param>
        /// <param name="locator">The <see cref="IElementLocator"/> implementation that
        /// determines how elements are located.</param>
        /// <param name="bys">The list of methods by which to search for the elements.</param>
        /// <param name="cacheLookups"><see langword="true"/> to cache the lookup to the element; otherwise, <see langword="false"/>.</param>
        /// <returns>An object used to proxy calls to properties and methods of the list of <see cref="IWebElement"/> objects.</returns>
        public static object CreateProxy(IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
        {
#if !NETSTANDARD2_0
            return new WebElementProxy(typeof(IWebElement), locator, bys, cacheLookups).GetTransparentProxy();
#else
            var proxy = Create<IWebElement, WebElementProxy>();
            ((WebElementProxy)(object)proxy).SetSearchProperites(locator, bys, cacheLookups);
            return proxy;
#endif
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Invokes the method that is specified in the provided <see cref="IMessage"/> on the
        /// object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/> that contains a dictionary of
        /// information about the method call. </param>
        /// <returns>The message returned by the invoked method, containing the return value and any
        /// out or ref parameters.</returns>
        public override IMessage Invoke(IMessage msg)
        {
            var element = this.Element;
            IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;

            if (typeof(IWrapsElement).IsAssignableFrom((methodCallMessage.MethodBase as MethodInfo).DeclaringType))
            {
                return new ReturnMessage(element, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
            }

            return WebDriverObjectProxy.InvokeMethod(methodCallMessage, element);
        }
#else
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var decalringType = targetMethod.DeclaringType;

            if (decalringType == typeof(IWebElement))
            {
                return targetMethod.Invoke(Element, args);
            }
            return targetMethod.Invoke(this, args);
        }
#endif
    }
}
