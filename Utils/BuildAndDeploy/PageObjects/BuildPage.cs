using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BuildAndDeploy.PageObjects
{
    public class BuildPage
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public BuildPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
            _logger.Trace("BuildPage(IWebDriver driver)");
        }

        [FindsBy(How = How.XPath, Using = "//form/table/tbody[34]/tr[1]/td[3]/div/input[2]")]
        public IWebElement Build_with_Parameters { get; set; }

        [FindsBy(How = How.Id, Using = "yui-gen1-button")]
        public IWebElement BuildButton { get; set; }


        public void TriggerBuild()
        {
            Build_with_Parameters.Clear();
            Build_with_Parameters.SendKeys("abhishek.goenka@allscripts.com");

            //trigger
            BuildButton.Click();
        }
    }
}