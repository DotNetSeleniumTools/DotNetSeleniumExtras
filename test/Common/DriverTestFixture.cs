using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumExtras.Environment;

namespace SeleniumExtras
{
    public abstract class DriverTestFixture
    {
        public string iframesPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("iframes.html");
        public string javascriptPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("javascriptPage.html");
        public string nestedPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("nestedElements.html");
        public string xhtmlTestPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("xhtmlTest.html");
        public string iframePage = EnvironmentManager.Instance.UrlBuilder.WhereIs("iframes.html");

        protected IWebDriver driver;

        public IWebDriver DriverInstance
        {
            get { return driver; }
            set { driver = value; }
        }

        public bool IsNativeEventsEnabled
        {
            get
            {
                IHasCapabilities capabilitiesDriver = driver as IHasCapabilities;
                if (capabilitiesDriver != null && capabilitiesDriver.Capabilities.HasCapability(CapabilityType.HasNativeEvents) && (bool)capabilitiesDriver.Capabilities.GetCapability(CapabilityType.HasNativeEvents))
                {
                    return true;
                }

                return false;
            }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = EnvironmentManager.Instance.GetCurrentDriver();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // EnvironmentManager.Instance.CloseCurrentDriver();
        }

        /*
         *  Exists because a given test might require a fresh driver
         */
        protected void CreateFreshDriver()
        {
            driver = EnvironmentManager.Instance.CreateFreshDriver();
        }

        protected bool IsIeDriverTimedOutException(Exception e)
        {
            // The IE driver may throw a timed out exception
            return e.GetType().Name.Contains("TimedOutException");
        }

        protected bool WaitFor(Func<bool> waitFunction, string timeoutMessage)
        {
            return WaitFor<bool>(waitFunction, timeoutMessage);
        }

        protected T WaitFor<T>(Func<T> waitFunction, string timeoutMessage)
        {
            return this.WaitFor<T>(waitFunction, TimeSpan.FromSeconds(5), timeoutMessage);
        }

        protected T WaitFor<T>(Func<T> waitFunction, TimeSpan timeout, string timeoutMessage)
        {
            DateTime endTime = DateTime.Now.Add(timeout);
            T value = default(T);
            Exception lastException = null;
            while (DateTime.Now < endTime)
            {
                try
                {
                    value = waitFunction();
                    if (typeof(T) == typeof(bool))
                    {
                        if ((bool)(object)value)
                        {
                            return value;
                        }
                    }
                    else if (value != null)
                    {
                        return value;
                    }

                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    // Swallow for later re-throwing
                    lastException = e;
                }
            }

            if (lastException != null)
            {
                throw new WebDriverException("Operation timed out", lastException);
            }

            Assert.Fail("Condition timed out: " + timeoutMessage);
            return default(T);
        }
    }
}
