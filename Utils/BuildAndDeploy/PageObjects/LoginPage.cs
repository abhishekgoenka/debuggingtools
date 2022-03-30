using BuildAndDeploy.SeleniumHelpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BuildAndDeploy.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "log in")]
        public IWebElement SignInLink { get; set; }

        [FindsBy(How = How.Id, Using = "j_username")]
        public IWebElement UserIdField { get; set; }

        [FindsBy(How = How.Name, Using = "j_password")]
        public IWebElement PasswordField { get; set; }

        /// <summary>
        /// JQuery selector example
        /// </summary>
        public IWebElement LoginButton => _driver.FindElement(By.Id("yui-gen1-button"));

        public void LoginAsAdmin(string baseUrl)
        {
            _driver.Navigate().GoToUrl(baseUrl);
            SignInLink.Click();
            UserIdField.Clear();
            // sending a single quote is not possible with the Chrome Driver, it sends two single quotes!
            UserIdField.SendKeys("agoenka");

            PasswordField.Clear();
            PasswordField.SendKeys("Computer1");

            LoginButton.Click();
        }

        public void LoginAsNobody(string baseUrl)
        {
            _driver.Navigate().GoToUrl(baseUrl);
            SignInLink.Click();
            UserIdField.Clear();
            // sending a single quote is not possible with the Chrome Driver, it sends two single quotes!
            UserIdField.SendKeys("nobody");

            PasswordField.Clear();
            PasswordField.SendKeys("blah");

            LoginButton.Click();
        }
    }
}