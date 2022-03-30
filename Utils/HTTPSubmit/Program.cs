#region

using System;

#endregion

namespace HTTPSubmit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var p = new TimedWebPageReaderProcessor();

            p.Process(args, null, Console.Out, Console.Error);
        }
    }
}