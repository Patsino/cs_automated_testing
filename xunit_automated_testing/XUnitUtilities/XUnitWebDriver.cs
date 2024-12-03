using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUITest.Utilities
{
    public class XUnitWebDriver
    {
        private TimeSpan _implicitWait = TimeSpan.FromSeconds(30);
        private bool _maximizeWindow = true;
        private string _link = "https://en.ehu.lt/";
        private IWebDriver? _driver;

        private XUnitWebDriver() { }

        public class WebDriverBuilder
        {
            private readonly XUnitWebDriver _webDriver;

            public WebDriverBuilder()
            {
                _webDriver = new XUnitWebDriver();
            }

            public WebDriverBuilder WithImplicitWait(TimeSpan timeout)
            {
                _webDriver._implicitWait = timeout;
                return this;
            }

            public WebDriverBuilder WithMaximizedWindow(bool maximize)
            {
                _webDriver._maximizeWindow = maximize;
                return this;
            }

            public WebDriverBuilder WithBaseUrl(string link)
            {
                _webDriver._link = link;
                return this;
            }

            public IWebDriver Build()
            {
                _webDriver._driver = new ChromeDriver();
                _webDriver._driver.Manage().Timeouts().ImplicitWait = _webDriver._implicitWait;
                if (_webDriver._maximizeWindow)
                {
                    _webDriver._driver.Manage().Window.Maximize();
                }

                _webDriver._driver.Navigate().GoToUrl(_webDriver._link);
                return _webDriver._driver;
            }
        }

        public static WebDriverBuilder CreateBuilder()
        {
            return new WebDriverBuilder();
        }
    }
}
