using Xunit;
using OpenQA.Selenium;
using WebUITest.Utilities;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 16)]

namespace WebUITest
{
    [Trait("Category", "Navigation")]
    public class NavigationTests : IDisposable
    {
        private readonly IWebDriver driver;

        public NavigationTests()
        {
            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();
        }

        [Fact]
        public void VerifyNavigationToAboutPage()
        {
            var aboutButton = driver.FindElement(By.LinkText("About"));
            aboutButton.Click();
            Assert.Equal("https://en.ehu.lt/about/", driver.Url);
            Assert.Equal("About", driver.Title);
            var header = driver.FindElement(By.TagName("h1"));
            Assert.Equal("About", header.Text);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    [Trait("Category", "LanguageSwitch")]
    public class LanguageSwitchTests : IDisposable
    {
        private readonly IWebDriver driver;

        public LanguageSwitchTests()
        {
            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();
        }

        [Fact]
        public void VerifyLanguageSwitchFunctionality()
        {
            var languageSwitchButton = driver.FindElement(By.CssSelector(".language-switcher"));
            languageSwitchButton.Click();
            var ltButton = driver.FindElement(By.LinkText("LT"));
            ltButton.Click();
            Assert.Equal("https://lt.ehu.lt/", driver.Url);
            var htmlTag = driver.FindElement(By.TagName("html"));
            string langAttribute = htmlTag.GetAttribute("lang");
            Assert.Equal("lt-LT", langAttribute);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    [Trait("Category", "Search")]
    public class SearchTests : IDisposable
    {
        private readonly IWebDriver driver;

        public SearchTests()
        {
            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();
        }

        [Theory]
        [InlineData("study programs", "study program")]
        public void VerifySearchFunctionalityWithDifferentQueries(string query, string expectedText)
        {
            var searchButton = driver.FindElement(By.ClassName("header-search"));
            searchButton.Click();
            var searchBar = driver.FindElement(By.CssSelector("input.form-control[name='s']"));
            searchBar.SendKeys(query);
            searchBar.SendKeys(Keys.Enter);

            Assert.Contains($"/?s={query.Replace(" ", "+")}", driver.Url);
            var searchResults = driver.FindElements(By.ClassName("content"));
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));
            Assert.True(resultsContainSearchTerm, $"Search results do not contain any expected text: {expectedText}");
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}