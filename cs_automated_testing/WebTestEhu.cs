using NUnit.Framework;
using OpenQA.Selenium;
using cs_automated_testing.Pages;
using cs_automated_testing.Utilities;
using FluentAssertions;
using Serilog;
using TechTalk.SpecFlow;

namespace WebUITest
{
    [Binding]
    public class EhuWebsiteSteps
    {
        private readonly IWebDriver driver;
        private readonly HomePage homePage;
        private readonly ILogger logger;

        public EhuWebsiteSteps()
        {
            logger = LoggerSetup.CreateLogger();
            logger.Information("Test execution started.");
            driver = WebDriverSingleton.GetDriver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
            logger.Debug("Browser window maximized.");
            homePage = new HomePage(driver);
        }

        [Given("I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            homePage.NavigateTo("https://en.ehu.lt/");
            logger.Information("Navigated to homepage: https://en.ehu.lt/");
        }

        [When("I click the About button")]
        public void WhenIClickTheAboutButton()
        {
            logger.Information("Clicking the 'About' button.");
            homePage.ClickAboutButton();
        }

        [Then("I should see the About page with the correct title and header")]
        public void ThenIShouldSeeTheAboutPageWithTheCorrectTitleAndHeader()
        {
            logger.Information("Verifying the About page.");
            homePage.GetUrl().Should().Be("https://en.ehu.lt/about/");
            homePage.GetTitle().Should().Be("About");
            homePage.GetPageHeader().Should().Be("About");
        }

        [When("I search for \"(.*)\"")]
        public void WhenISearchFor(string query)
        {
            logger.Information($"Searching for query: {query}");
            homePage.ClickSearchButton();
            homePage.EnterSearchQuery(query);
        }

        [Then("I should see search results containing \"(.*)\"")]
        public void ThenIShouldSeeSearchResultsContaining(string expectedText)
        {
            logger.Information($"Verifying search results contain: {expectedText}");
            homePage.HasSearchResults(expectedText).Should().BeTrue();
        }

        [When("I switch the language to Lithuanian")]
        public void WhenISwitchTheLanguageToLithuanian()
        {
            logger.Information("Switching the language to Lithuanian.");
            homePage.SwitchToLithuanianLanguage();
        }

        [Then("the URL should be \"(.*)\"")]
        public void ThenTheUrlShouldBe(string expectedUrl)
        {
            logger.Information($"Verifying URL is: {expectedUrl}");
            homePage.GetUrl().Should().Be(expectedUrl);
        }

        [Then("the page's lang attribute should be \"(.*)\"")]
        public void ThenThePagesLangAttributeShouldBe(string expectedLang)
        {
            logger.Information($"Verifying lang attribute is: {expectedLang}");
            var htmlTag = driver.FindElement(By.TagName("html"));
            htmlTag.GetAttribute("lang").Should().Be(expectedLang);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            logger.Information("Tearing down the browser.");
            WebDriverSingleton.QuitDriver();
            logger.Information("Browser closed and WebDriver disposed.");
        }
    }
}