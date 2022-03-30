using System;
using System.Linq;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BuildAndDeploy
{
    public class BuildRunner : IDisposable
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private const string JENKINS_URL = "http://tw-cm-util-01:8080/";
        private readonly IWebDriver driver;
        private readonly string websiteLink;
        public BuildRunner(String websiteLink)
        {
            driver = new ChromeDriver();
            this.websiteLink = websiteLink;
        }
        public void OpenJenkinsURL()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(websiteLink);

            //logout if already login. 
            LogoutAndLogin();

            _logger.Trace("Opened Jenkins URL");
        }

        private void LogoutAndLogin()
        {
            IWebElement element = driver.FindElement(By.LinkText("log in"));
            element?.Click();

            //wait for login page to load
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            //username 
            driver.FindElement(By.Id("j_username")).SendKeys("agoenka");

            //set password
            driver.FindElement(By.Name("j_password")).SendKeys("Computer11");

            driver.FindElement(By.Id("yui-gen1-button")).Click();

            _logger.Trace("Login successful");

        }

        public void ClickLinkText(string text)
        {
            driver.FindElement(By.LinkText(text)).Click();
        }

        public void ClickById(string text)
        {
            driver.FindElement(By.Id(text)).Click();
        }

        public void Wait(Int32 seconds)
        {
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(seconds));
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));
        }

        public void SetText(By findBy, String newtext)
        {
            var element = driver.FindElements(findBy);
            var textbox = element.Last();

            //clear and set value.
            textbox.Clear();
            textbox.SendKeys(newtext);
            _logger.Trace("SetText to new : " + newtext);

        }

        public void Dispose()
        {
            driver.Dispose();
        }
    }
}