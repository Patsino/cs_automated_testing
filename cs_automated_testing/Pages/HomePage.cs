using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace cs_automated_testing.Pages
{
    public class HomePage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By AboutButton = By.LinkText("About");
        private readonly By SearchButton = By.ClassName("header-search");
        private readonly By SearchBar = By.CssSelector("input.form-control[name='s']");
        private readonly By SearchResults = By.ClassName("content");
        private readonly By LanguageSwitchButton = By.CssSelector(".language-switcher");
        private readonly By LithuanianLanguageButton = By.LinkText("LT");

        public void ClickAboutButton()
        {
            Driver.FindElement(AboutButton).Click();
        }

        public void ClickSearchButton()
        {
            Driver.FindElement(SearchButton).Click();
        }

        public void EnterSearchQuery(string query)
        {
            Driver.FindElement(SearchBar).SendKeys(query);
            Driver.FindElement(SearchBar).SendKeys(Keys.Enter);
        }

        public bool HasSearchResults(string expectedText)
        {
            var searchResults = Driver.FindElements(SearchResults);
            return searchResults.Any(result => result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));
        }

        public void SwitchToLithuanianLanguage()
        {
            Driver.FindElement(LanguageSwitchButton).Click();
            Driver.FindElement(LithuanianLanguageButton).Click();
        }
    }
}
