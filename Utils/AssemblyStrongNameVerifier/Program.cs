using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AssemblyStrongNameVerifier
{
    internal class Program
    {
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        private static extern bool StrongNameSignatureVerificationEx(string wszFilePath, bool fForceVerification,
            ref bool pfWasVerified);

        private static void Main(string[] args)
        {
            foreach (
                var file in Directory.GetFiles(@"C:\branches\Infrared\Shared\lib\Shield", "*.dll"))
            {
                var notForced = false;
                var assembly = file;
                var verified = StrongNameSignatureVerificationEx(assembly, false, ref notForced);

                if (verified && notForced)
                    Console.WriteLine("Strong name signature");
                else
                    Console.WriteLine("Strong name signature not found");
            }
        }
    }
}