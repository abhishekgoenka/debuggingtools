using System;
using BuildAndDeploy.Build;
using BuildAndDeploy.JenkinsJob;
using static System.Console;

namespace BuildAndDeploy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                int selectedMenu;
                var jenkins = new Jenkins();
                do
                {
                    WriteLine("1. Trigger Infrared Build");
                    WriteLine("2. Open MAIN Infrared CI");
                    WriteLine("3. Open MAIN CI");
                    WriteLine("4. Open Infrared build page");
                    WriteLine("9. Exit");
                    WriteLine("Select your choice : ");
                    var input = ReadLine();

                    if (int.TryParse(input, out selectedMenu))
                    {
                        switch (selectedMenu)
                        {
                            case 1:
                                jenkins.Execute(new InfraredBuild());
                                break;

                            case 2:
                                System.Diagnostics.Process.Start("http://alm-prod-app1.rd.allscripts.com:8080/tfs/boc_projects/TWEHR/_build?_a=completed&definitionId=3295");
                                break;

                            case 3:
                                System.Diagnostics.Process.Start("http://alm-prod-app1.rd.allscripts.com:8080/tfs/boc_projects/TWEHR/_build?_a=completed&definitionId=3175");
                                break;

                            case 4:
                                System.Diagnostics.Process.Start("http://tw-cm-util-01:8080/view/17.1.x/job/Build_TWEHR_17.1.0_Infrared/");
                                break;

                            case 9:
                                break;
                        }
                    }
                    Clear();
                } while (selectedMenu != 9);
            }
            catch (Exception e)
            {
                WriteLine(e);
                ReadKey();
            }
        }
    }
}