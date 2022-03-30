using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BuildAndDeploy.PageObjects
{
    public class Build_TWEHR_1710_InfraredPage
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebDriver _driver;

        public Build_TWEHR_1710_InfraredPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
            _logger.Trace("Build_TWEHR_1710_InfraredPage(IWebDriver driver)");
        }

        [FindsBy(How = How.LinkText, Using = "Build with Parameters")]
        public IWebElement Build_with_Parameters { get; set; }

        public BuildPage NavigateToBuildPage()
        {
            Build_with_Parameters.Click();
            return new BuildPage(_driver);
        }
    }
}