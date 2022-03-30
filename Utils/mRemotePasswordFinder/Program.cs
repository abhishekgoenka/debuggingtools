using System;
using CommandLine;
using static System.Console;

namespace mRemotePasswordFinder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args);

            if (result.Tag == ParserResultType.Parsed)
            {
                String password = ((Parsed<Options>) result).Value.Password;
                WriteLine($"Password is : {password}");
            }
        }
    }
}