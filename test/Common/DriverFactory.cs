using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace SeleniumExtras.Environment
{
    public class DriverFactory
    {
        private const string host = "127.0.0.1";

        public DriverFactory(string driverPath)
        {
            if (string.IsNullOrEmpty(driverPath))
            {
                this.DriverServicePath = TestContext.CurrentContext.TestDirectory;
            }
            else
            {
                this.DriverServicePath = driverPath;
            }
        }

        public string DriverServicePath { get; }

        public IWebDriver CreateDriver(Type driverType)
        {
            List<Type> constructorArgTypeList = new List<Type>();
            IWebDriver driver = null;
            if (typeof(ChromeDriver).IsAssignableFrom(driverType))
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(this.DriverServicePath);
                service.HostName = host;
                constructorArgTypeList.Add(typeof(ChromeDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(InternetExplorerDriver).IsAssignableFrom(driverType))
            {
                InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService(this.DriverServicePath);
                service.HostName = host;
                constructorArgTypeList.Add(typeof(InternetExplorerDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(EdgeDriver).IsAssignableFrom(driverType))
            {
                EdgeDriverService service = EdgeDriverService.CreateDefaultService(this.DriverServicePath);
                service.HostName = host;
                constructorArgTypeList.Add(typeof(EdgeDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(FirefoxDriver).IsAssignableFrom(driverType))
            {
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(this.DriverServicePath);
                service.HostName = host;
                constructorArgTypeList.Add(typeof(FirefoxDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            driver = (IWebDriver)Activator.CreateInstance(driverType);
            return driver;
        }
    }
}
