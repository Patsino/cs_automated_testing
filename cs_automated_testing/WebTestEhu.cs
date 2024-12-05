using NUnit.Framework;
using OpenQA.Selenium;
using cs_automated_testing.Pages;
using cs_automated_testing.Utilities;

namespace WebUITest
{
    [TestFixture]
    public class WebTestEhu
    {
        private IWebDriver? driver;
        private HomePage? homePage;

        [SetUp]
        public void SetUp()
        {
            driver = WebDriverSingleton.GetDriver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
            homePage = new HomePage(driver);
            homePage.NavigateTo("https://en.ehu.lt/");
        }

        [Test, Category("Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            if (homePage == null)
            {
                Assert.Fail("HomePage object is null.");
                throw new ArgumentNullException($"{driver} is null");
            }

            homePage.ClickAboutButton();
            Assert.That(homePage.GetUrl(), Is.EqualTo("https://en.ehu.lt/about/"), "The URL does not match the expected value.");
            Assert.That(homePage.GetTitle(), Is.EqualTo("About"), "The page title does not match the expected value.");
            Assert.That(homePage.GetPageHeader(), Is.EqualTo("About"), "The header text does not match the expected value.");
        }

        [Test, Category("Search")]
        [TestCase("study programs", "study program")]
        public void VerifySearchFunctionality(string query, string expectedText)
        {
            if (homePage == null)
            {
                Assert.Fail("HomePage object is null.");
                throw new ArgumentNullException($"{driver} is null");
            }

            homePage.ClickSearchButton();
            homePage.EnterSearchQuery(query);
            Assert.That(driver?.Url, Does.Contain($"/?s={query.Replace(" ", "+")}"));
            Assert.That(homePage.HasSearchResults(expectedText), Is.True, $"Search results do not contain any expected text: {expectedText}");
        }

        [Test, Category("LanguageSwitch")]
        public void VerifyLanguageSwitchFunctionality()
        {
            if (homePage == null)
            {
                Assert.Fail("HomePage object is null.");
                throw new ArgumentNullException($"{driver} is null");
            }

            homePage.SwitchToLithuanianLanguage();
            Assert.That(homePage.GetUrl(), Is.EqualTo("https://lt.ehu.lt/"), "The URL does not match the expected value.");
            var htmlTag = driver?.FindElement(By.TagName("html"));
            string langAttribute = htmlTag?.GetAttribute("lang") ?? string.Empty;
            Assert.That(langAttribute, Is.EqualTo("lt-LT"), "The lang attribute is not equal to lt-LT.");
        }

        [TearDown]
        public void Teardown()
        {
            WebDriverSingleton.QuitDriver();
        }
    }
}
