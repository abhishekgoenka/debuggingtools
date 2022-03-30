using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace BuildAndDeploy.SeleniumHelpers
{
    class ExtendedRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
    {
        private readonly Uri _remoteHost;

        public ExtendedRemoteWebDriver(Uri remoteHost, ICapabilities capabilities, TimeSpan commandTimeout)
            : base(remoteHost, capabilities, commandTimeout)
        {
            _remoteHost = remoteHost;
        }

        public string GetNodeHost()
        {
            var result = "[UNKNOWN HOST]";
            var uri = new Uri($"http://{_remoteHost.Host}:{_remoteHost.Port}/grid/api/testsession?session={SessionId}");

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var httpResponse = (HttpWebResponse)request.GetResponse())
            {
                var stream = httpResponse.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = JObject.Parse(reader.ReadToEnd()).SelectToken("proxyId").ToString();
                    }
                }
            }
            return result;
        }

        //this will allow screenshots to be taken from the remote browser
        public Screenshot GetScreenshot()
        {
            return new Screenshot(Execute(DriverCommand.Screenshot, null).Value.ToString());
        }
    }
}