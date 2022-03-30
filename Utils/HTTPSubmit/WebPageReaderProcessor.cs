#region

using System;
using System.IO;
using System.Net;

#endregion

namespace HTTPSubmit
{
    public class WebPageReaderProcessor : ProcessorTemplateBase<WebPageReaderOptions>
    {
        protected override void PreProcess()
        {
            Output.WriteLine("Downloading page...");

            var wc = new WebClient();

            String page = String.Empty;
            try
            {
                page = wc.DownloadString(Options.Uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Input = new StringReader(page);
        }

        protected override void ProcessLine(string line)
        {
            Output.WriteLine(line);
        }
    }
}