using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Compatibility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[assembly: Parallelizable(ParallelScope.All)]

namespace WebUITest
{
    [TestFixture]
    public class WebTestEhu
    {
        protected ThreadLocal<IWebDriver> _driver = new();
        protected IWebDriver? driver => _driver.Value;

        [SetUp]
        public void SetUp()
        {
            _driver.Value = new ChromeDriver();
            _driver.Value.Manage().Window.Maximize();
        }

        [Test, Category("Navigation")]
        [Parallelizable]
        public void VerifyNavigationToAboutPage()
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var aboutButton = driver.FindElement(By.XPath("//*[@id=\"menu-item-16178\"]/a"));
            aboutButton.Click();
            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/about/"), "The URL does not match the expected value.");
            Assert.That(driver.Title, Is.EqualTo("About"), "The page title does not match the expected value.");
            var header = driver.FindElement(By.TagName("h1"));
            Assert.That(header.Text, Is.EqualTo("About"), "The header text does not match the expected value.");
        }


        [Test, Category("Search")]
        [TestCase("study programs", "study program")]
        [Parallelizable]
        public void VerifySearchFunctionality(string query, string expectedText)
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();
            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(query);
            searchBar.SendKeys(Keys.Enter);
            Assert.That(driver.Url, Does.Contain($"/?s={query.Replace(" ", "+")}"));
            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));
            Assert.That(resultsContainSearchTerm, Is.True, $"Search results do not contain any expected text: {expectedText}");
        }


        [Test, Category("LanguageSwitch")]
        [Parallelizable]
        public void VerifyLanguageSwitchFunctionality()
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var languageSwitchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();
            var ltButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();
            Assert.That(driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "The URL does not match the expected value.");
            var htmlTag = driver.FindElement(By.TagName("html"));
            string langAttribute = htmlTag.GetAttribute("lang");
            Assert.That(langAttribute, Is.EqualTo("lt-LT"), "The lang attribute is not equal to lt-LT.");
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Value?.Quit();
            _driver.Value?.Dispose();
        }
    }
}
