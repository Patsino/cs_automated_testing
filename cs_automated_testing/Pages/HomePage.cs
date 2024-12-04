using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace cs_automated_testing.Pages
{
    public class HomePage : BasePage
    {
        private readonly By AboutButton = By.XPath("//*[@id=\"menu-item-16178\"]/a");
        private readonly By SearchButton = By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div");
        private readonly By SearchBar = By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input");
        private readonly By LanguageSwitchButton = By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul");
        private readonly By LithuanianLanguageButton = By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a");

        public HomePage(IWebDriver driver) : base(driver) { }

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

        public void SwitchToLithuanianLanguage()
        {
            Driver.FindElement(LanguageSwitchButton).Click();
            Driver.FindElement(LithuanianLanguageButton).Click();
        }
    }
}
