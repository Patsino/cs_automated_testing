using Xunit;
using OpenQA.Selenium;
using WebUITest.Utilities;
using FluentAssertions;
using Serilog;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 16)]

namespace WebUITest
{
    [Trait("Category", "Navigation")]
    public class NavigationTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly ILogger logger;

        public NavigationTests()
        {
            logger = LoggerSetup.CreateLogger();
            logger.Information("Initializing NavigationTests...");

            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();

            logger.Debug("WebDriver initialized with base URL: https://en.ehu.lt/");
        }

        [Fact]
        public void VerifyNavigationToAboutPage()
        {
            logger.Information("Test: VerifyNavigationToAboutPage started.");

            var aboutButton = driver.FindElement(By.LinkText("About"));
            aboutButton.Click();
            logger.Debug("Clicked on 'About' button.");

            driver.Url.Should().Be("https://en.ehu.lt/about/", "The URL should navigate to the About page.");
            driver.Title.Should().Be("About", "The page title should match the About page.");

            var header = driver.FindElement(By.TagName("h1"));
            header.Text.Should().Be("About", "The header text should match the expected value.");
            logger.Information("Test: VerifyNavigationToAboutPage passed.");
        }

        public void Dispose()
        {
            logger.Information("Disposing resources for NavigationTests...");
            driver.Quit();
            driver.Dispose();
            logger.Information("WebDriver disposed.");
        }
    }

    [Trait("Category", "LanguageSwitch")]
    public class LanguageSwitchTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly ILogger logger;

        public LanguageSwitchTests()
        {
            logger = LoggerSetup.CreateLogger();
            logger.Information("Initializing LanguageSwitchTests...");

            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();

            logger.Debug("WebDriver initialized with base URL: https://en.ehu.lt/");
        }

        [Fact]
        public void VerifyLanguageSwitchFunctionality()
        {
            logger.Information("Test: VerifyLanguageSwitchFunctionality started.");

            var languageSwitchButton = driver.FindElement(By.CssSelector(".language-switcher"));
            languageSwitchButton.Click();
            logger.Debug("Clicked on language switcher.");

            var ltButton = driver.FindElement(By.LinkText("LT"));
            ltButton.Click();
            logger.Debug("Switched to Lithuanian language.");

            driver.Url.Should().Be("https://lt.ehu.lt/", "The URL should switch to the Lithuanian homepage.");
            var htmlTag = driver.FindElement(By.TagName("html"));
            string langAttribute = htmlTag.GetAttribute("lang");
            langAttribute.Should().Be("lt-LT", "The lang attribute should indicate Lithuanian language.");
            logger.Information("Test: VerifyLanguageSwitchFunctionality passed.");
        }

        public void Dispose()
        {
            logger.Information("Disposing resources for LanguageSwitchTests...");
            driver.Quit();
            driver.Dispose();
            logger.Information("WebDriver disposed.");
        }
    }

    [Trait("Category", "Search")]
    public class SearchTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly ILogger logger;

        public SearchTests()
        {
            logger = LoggerSetup.CreateLogger();
            logger.Information("Initializing SearchTests...");

            driver = XUnitWebDriver.CreateBuilder()
                .WithImplicitWait(TimeSpan.FromSeconds(30))
                .WithMaximizedWindow(true)
                .WithBaseUrl("https://en.ehu.lt/")
                .Build();

            logger.Debug("WebDriver initialized with base URL: https://en.ehu.lt/");
        }

        [Theory]
        [InlineData("study programs", "study program")]
        public void VerifySearchFunctionalityWithDifferentQueries(string query, string expectedText)
        {
            logger.Information($"Test: VerifySearchFunctionalityWithDifferentQueries started. Query: {query}");

            var searchButton = driver.FindElement(By.ClassName("header-search"));
            searchButton.Click();
            logger.Debug("Clicked on search button.");

            var searchBar = driver.FindElement(By.CssSelector("input.form-control[name='s']"));
            searchBar.SendKeys(query);
            searchBar.SendKeys(Keys.Enter);
            logger.Debug($"Entered search query: {query}");

            driver.Url.Should().Contain($"/?s={query.Replace(" ", "+")}", "The URL should include the search query.");
            var searchResults = driver.FindElements(By.ClassName("content"));
            searchResults.Should().NotBeNullOrEmpty("Search results should not be empty.");

            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));
            resultsContainSearchTerm.Should().BeTrue($"Search results should contain the expected text: {expectedText}");
            logger.Information("Test: VerifySearchFunctionalityWithDifferentQueries passed.");
        }

        public void Dispose()
        {
            logger.Information("Disposing resources for SearchTests...");
            driver.Quit();
            driver.Dispose();
            logger.Information("WebDriver disposed.");
        }
    }
}