
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace cs_automated_testing.Utilities
{
    public class WebDriverSingleton
    {
        private static IWebDriver? _driver;

        private static readonly object Lock = new();

        private WebDriverSingleton() { }

        public static IWebDriver GetDriver
        {
            get
            {
                if (_driver == null)
                {
                    lock (Lock)
                    {
                        _driver ??= new ChromeDriver();
                    }
                }
                return _driver;
            }
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
