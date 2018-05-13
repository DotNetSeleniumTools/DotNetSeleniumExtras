using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumExtras.Environment
{
    public class EnvironmentManager
    {
        private static EnvironmentManager instance;
        private Type driverType;
        private IWebDriver driver;
        private DriverFactory driverFactory;

        private EnvironmentManager()
        {
            string currentDirectory = this.CurrentDirectory;
            string content = File.ReadAllText(Path.Combine(currentDirectory, "appconfig.json"));
            TestEnvironment env = JsonConvert.DeserializeObject<TestEnvironment>(content);

            string activeDriverConfig = TestContext.Parameters.Get("ActiveDriverConfig", env.ActiveDriverConfig);
            string activeWebsiteConfig = TestContext.Parameters.Get("ActiveWebsiteConfig", env.ActiveWebsiteConfig);
            string driverServiceLocation = TestContext.Parameters.Get("DriverServiceLocation", env.DriverServiceLocation);
            DriverConfig driverConfig = env.DriverConfigs[activeDriverConfig];
            WebsiteConfig websiteConfig = env.WebSiteConfigs[activeWebsiteConfig];
            this.driverFactory = new DriverFactory(driverServiceLocation);

            Assembly driverAssembly = Assembly.Load(driverConfig.AssemblyName);
            driverType = driverAssembly.GetType(driverConfig.DriverTypeName);
            Browser = driverConfig.BrowserValue;
            RemoteCapabilities = driverConfig.RemoteCapabilities;

            UrlBuilder = new UrlBuilder(websiteConfig);

            WebServer = new TestWebServer(UrlBuilder.BaseUrl, currentDirectory);
        }

        ~EnvironmentManager()
        {
            WebServer?.Stop();
            driver?.Quit();
        }

        public Browser Browser { get; private set; }

        public string DriverServiceDirectory
        {
            get { return this.driverFactory.DriverServicePath; }
        }

        public string CurrentDirectory
        {
            get
            {
                return Path.GetDirectoryName(typeof(EnvironmentManager).Assembly.Location);
            }
        }

        public TestWebServer WebServer { get; private set; }

        public string RemoteCapabilities { get; private set; }

        public IWebDriver GetCurrentDriver()
        {
            return driver ?? CreateFreshDriver();
        }

        public IWebDriver CreateDriverInstance()
        {
            return driverFactory.CreateDriver(driverType);
        }

        public IWebDriver CreateFreshDriver()
        {
            CloseCurrentDriver();
            driver = CreateDriverInstance();
            return driver;
        }

        public void CloseCurrentDriver()
        {
            driver?.Quit();
            driver = null;
        }

        public static EnvironmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnvironmentManager();
                }

                return instance;
            }
        }

        public UrlBuilder UrlBuilder { get; private set; }

    }
}
