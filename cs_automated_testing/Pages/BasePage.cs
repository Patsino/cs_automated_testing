using OpenQA.Selenium;

namespace cs_automated_testing.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public string GetTitle()
        {
            return Driver.Title;
        }

        public string GetUrl()
        {
            return Driver.Url;
        }

        public string GetPageHeader()
        {
            return Driver.FindElement(By.TagName("h1")).Text;
        }
    }
}
