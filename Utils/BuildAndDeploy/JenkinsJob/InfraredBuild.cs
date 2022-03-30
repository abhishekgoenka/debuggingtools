using BuildAndDeploy.Build;
using BuildAndDeploy.PageObjects;

namespace BuildAndDeploy.JenkinsJob
{
    /// <summary>
    /// Trigger infrared build
    /// </summary>
    class InfraredBuild : Command
    {
        public override void Trigger()
        {
            //login
            LoginAsAbhishek();

            new MainPage(_driver).NavigateToBuild_TWEHR_1710_Infrared().NavigateToBuildPage().TriggerBuild();
        }
    }
}