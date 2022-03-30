using CommandLine;

namespace mRemotePasswordFinder
{
    /// <summary>
    ///     Contains command line options
    /// </summary>
    public class Options
    {
        [Option(longName: "password", Required = true, HelpText = "password field of mRemote")]
        public string Password { get; set; }
    }
}