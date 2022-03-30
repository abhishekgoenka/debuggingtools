#region

using CommandLine;

#endregion

namespace HTTPSubmit
{
    public class WebPageReaderOptions
    {
        [Option('u', "uri",
            DefaultValue =
                "http://localhost/cmsv4/CEDService.asmx/CreateDocumentForPatients?&ClientSTDTZ=Central Standard Time&ServerSTDTZ=Central Standard Time&AppGroup=TOUCHWORKS",
            HelpText = "Enter HTTP URL to process")]
        public string Uri { get; set; }
    }
}