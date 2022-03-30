using System.Threading;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BuildAndDeploy.PageObjects
{
    /// <summary>
    /// All Jenkins tab
    /// </summary>
    public class MainPage
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebDriver _driver;

        public MainPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
            _logger.Trace("AllPage(IWebDriver driver)");
        }

        [FindsBy(How = How.LinkText, Using = "All")]
        public IWebElement AllTab { get; set; }

        [FindsBy(How = How.LinkText, Using = "Build_TWEHR_17.1.0_Infrared")]
        public IWebElement Build_TWEHR_1710_Infrared { get; set; }

        private void NavigateToAllTab()
        {
            AllTab.Click();
            
            //return new TransferFundsPage(_driver);
        }

        public Build_TWEHR_1710_InfraredPage NavigateToBuild_TWEHR_1710_Infrared()
        {
            NavigateToAllTab();
            Thread.Sleep(2000);
            Build_TWEHR_1710_Infrared.Click();
            return new Build_TWEHR_1710_InfraredPage(_driver);
        }
    }
}