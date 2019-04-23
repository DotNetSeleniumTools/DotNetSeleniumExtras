using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumExtras.PageObjects.Linq
{
    public class WebElementListProxyTests
    {
        private Mock<IWebElement> elementMock;
        private Mock<ISearchContext> driverMock;
        private By by;

        [SetUp]
        public void SetupMocks()
        {
            by = By.XPath(".//div");

            elementMock = new Mock<IWebElement>();
            elementMock.Setup(_ => _.Text).Returns("text");

            driverMock = new Mock<ISearchContext>();
            driverMock.Setup(_ => _.FindElements(by)).Returns(new ReadOnlyCollection<IWebElement>(Enumerable.Repeat(elementMock.Object, 10).ToList()));
        }

        [Test]
        public void LinqSelectShouldRetrieveElementsOnce()
        {
            var elements = new WebElementListProxy(new DefaultElementLocator(driverMock.Object), new[] { by }, false);

            var texts = elements.Select(e => e.Text).ToList();

            driverMock.Verify(_ => _.FindElements(by), Times.Once);
        }

        [Test]
        public void LinqSelectManyShouldRetrieveElementsOnce()
        {
            var elements = new WebElementListProxy(new DefaultElementLocator(driverMock.Object), new[] { by }, false);

            var texts = elements.SelectMany(e => e.Text).ToList();

            driverMock.Verify(_ => _.FindElements(by), Times.Once);
        }

        [Test]
        public void LinqWhereShouldRetrieveElementsOnce()
        {
            var elements = new WebElementListProxy(new DefaultElementLocator(driverMock.Object), new[] { by }, false);

            var texts = elements.Where(e => e.Text == "a").ToList();

            driverMock.Verify(_ => _.FindElements(by), Times.Once);
        }

        [Test]
        public void LinqToListShouldRetrieveElementsOnce()
        {
            var elements = new WebElementListProxy(new DefaultElementLocator(driverMock.Object), new[] { by }, false);

            var texts = elements.ToList();

            driverMock.Verify(_ => _.FindElements(by), Times.Once);
        }

        [Test]
        public void LinqToArrayShouldRetrieveElementsOnce()
        {
            var elements = new WebElementListProxy(new DefaultElementLocator(driverMock.Object), new[] { by }, false);

            var texts = elements.ToArray();

            driverMock.Verify(_ => _.FindElements(by), Times.Once);
        }
    }
}