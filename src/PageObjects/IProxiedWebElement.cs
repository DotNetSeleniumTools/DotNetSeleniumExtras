using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace SeleniumExtras
{
    internal interface IProxiedWebElement : IWebElement, IWrapsElement, ILocatable
    {
    }
}
