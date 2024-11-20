using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Compatibility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUITest
{
    [TestFixture]
    public class WebTestEhu
    {
        private IWebDriver? driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyNavigationToAboutPage()
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"menu-item-16178\"]/a"));
            searchButton.Click();
            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/about/"), "The URL does not match the expected value.");
            Assert.That(driver.Title, Is.EqualTo("About"), "The page title does not match the expected value.");
            var header = driver.FindElement(By.TagName("h1"));
            Assert.That(header.Text, Is.EqualTo("About"), "The header text does not match the expected value.");
        }


        [Test]
        public void VerifySearchFunctionality()
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();
            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys("study programs");
            searchBar.SendKeys(Keys.Enter);

            Assert.That(driver.Url, Does.Contain("/?s=study+programs"), "The URL does not contain the expected search query.");
            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains("study program", StringComparison.OrdinalIgnoreCase));
            Assert.That(resultsContainSearchTerm, Is.True, "Search results do not contain any study programs.");
        }

        [Test]
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

        //[Test]
        //public void VerifyContactFormSubmission
        // no form on website

        [TearDown]
        public void TearDown()
        {
            if (driver == null)
            {
                throw new ArgumentNullException($"{driver} is null");
            }

            driver.Quit();
        }
    }
}
