using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUITest
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; private set; }

        public WebDriverFixture()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }

    public class XUnitWebTestEhu : IClassFixture<WebDriverFixture>
    {
        private readonly IWebDriver driver;

        public XUnitWebTestEhu(WebDriverFixture fixture)
        {
            driver = fixture.Driver;
        }

        [Fact]
        [Trait("Category", "Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var aboutButton = driver.FindElement(By.XPath("//*[@id=\"menu-item-16178\"]/a"));
            aboutButton.Click();

            Assert.Equal("https://en.ehu.lt/about/", driver.Url);
            Assert.Equal("About", driver.Title);

            var header = driver.FindElement(By.TagName("h1"));
            Assert.Equal("About", header.Text);
        }

       
        [Fact]
        [Trait("Category", "LanguageSwitch")]
        public void VerifyLanguageSwitchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var languageSwitchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();

            var ltButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();

            Assert.Equal("https://lt.ehu.lt/", driver.Url);

            var htmlTag = driver.FindElement(By.TagName("html"));
            string langAttribute = htmlTag.GetAttribute("lang");
            Assert.Equal("lt-LT", langAttribute);
        }

        [Theory]
        [Trait("Category", "Navigation")]
        [InlineData("study programs", "study program")]
        public void VerifySearchFunctionalityWithDifferentQueries(string query, string expectedText)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();

            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(query);
            searchBar.SendKeys(Keys.Enter);

            Assert.Contains($"/?s={query.Replace(" ", "+")}", driver.Url);

            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));
            bool resultsContainSearchTerm = searchResults.Any(result =>
                result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));

            Assert.True(resultsContainSearchTerm, $"Search results do not contain any expected text: {expectedText}");
        }
    }
}
