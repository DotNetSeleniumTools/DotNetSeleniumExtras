using OpenQA.Selenium;

namespace SeleniumExtras
{
    internal interface IProxiedWebElement : IWebElement, IWrapsElement, ILocatable
    {
    }
}
