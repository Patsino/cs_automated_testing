using NUnit.Framework;
using OpenQA.Selenium;
using cs_automated_testing.Pages;
using cs_automated_testing.Utilities;
using FluentAssertions;
using Serilog;

namespace WebUITest
{
    [TestFixture]
    public class WebTestEhu
    {
        private IWebDriver? driver;
        private HomePage? homePage;
        private ILogger? logger;

        [SetUp]
        public void SetUp()
        {
            logger = LoggerSetup.CreateLogger();
            logger.Information("Test setup started.");

            driver = WebDriverSingleton.GetDriver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
            logger.Debug("Window maximized.");

            homePage = new HomePage(driver);
            homePage.NavigateTo("https://en.ehu.lt/");
            logger.Information("Navigated to homepage: https://en.ehu.lt/");
        }

        [Test, Category("Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            logger.Information("Test case: VerifyNavigationToAboutPage started.");

            homePage.Should().NotBeNull("HomePage object must be initialized.");

            homePage.ClickAboutButton();
            logger.Debug("Clicked 'About' button.");

            string currentUrl = homePage.GetUrl();
            currentUrl.Should().Be("https://en.ehu.lt/about/", "The URL must match the About page.");
            logger.Information($"Verified the URL is correct: {currentUrl}");

            string pageTitle = homePage.GetTitle();
            pageTitle.Should().Be("About", "The page title must match.");
            logger.Information($"Verified the page title is correct: {pageTitle}");

            string pageHeader = homePage.GetPageHeader();
            pageHeader.Should().Be("About", "The header text must match.");
            logger.Information($"Verified the header text is correct: {pageHeader}");

            logger.Information("Test case: VerifyNavigationToAboutPage passed.");
        }

        [Test, Category("Search")]
        [TestCase("study programs", "study program")]
        public void VerifySearchFunctionality(string query, string expectedText)
        {
            logger.Information($"Test case: VerifySearchFunctionality started. Query: {query}");

            homePage.Should().NotBeNull("HomePage object must be initialized.");
            homePage.ClickSearchButton();
            logger.Debug("Clicked 'Search' button.");

            homePage.EnterSearchQuery(query);
            logger.Debug($"Entered search query: {query}");

            string currentUrl = driver?.Url;
            currentUrl.Should().Contain($"/?s={query.Replace(" ", "+")}", "The search URL should include the query string.");
            logger.Information($"Verified the search URL contains the query string: {currentUrl}");

            bool resultsContainSearchTerm = homePage.HasSearchResults(expectedText);
            resultsContainSearchTerm.Should().BeTrue($"Search results should contain the expected text: {expectedText}");
            logger.Information("Search functionality verified successfully.");
        }

        [Test, Category("LanguageSwitch")]
        public void VerifyLanguageSwitchFunctionality()
        {
            logger.Information("Test case: VerifyLanguageSwitchFunctionality started.");

            homePage.Should().NotBeNull("HomePage object must be initialized.");
            homePage.SwitchToLithuanianLanguage();
            logger.Debug("Switched language to Lithuanian.");

            string currentUrl = homePage.GetUrl();
            currentUrl.Should().Be("https://lt.ehu.lt/", "The URL should match the Lithuanian homepage.");
            logger.Information($"Verified the URL is correct: {currentUrl}");

            var htmlTag = driver?.FindElement(By.TagName("html"));
            string langAttribute = htmlTag?.GetAttribute("lang") ?? string.Empty;
            langAttribute.Should().Be("lt-LT", "The lang attribute should indicate Lithuanian language.");
            logger.Information($"Verified the lang attribute is correct: {langAttribute}");

            logger.Information("Language switch functionality verified successfully.");
        }

        [TearDown]
        public void Teardown()
        {
            logger?.Information("Test teardown started.");
            WebDriverSingleton.QuitDriver();
            logger?.Information("Browser closed and WebDriver disposed.");
        }
    }
}