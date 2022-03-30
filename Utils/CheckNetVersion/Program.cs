using System;
using Microsoft.Win32;

namespace CheckNetVersion
{
    class Program
    {
        static void Main()
        {
            try
            {
                GetVersionFromRegistry();

                Get45or451FromRegistry();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
            
            //halt key
            Console.ReadKey();

        }

        /// <summary>
        /// The example produces output that's similar to the following:
        ///     v2.0.50727  2.0.50727.4016  SP2
        ///     v3.0  3.0.30729.4037  SP2
        ///     v3.5  3.5.30729.01  SP1
        ///     v4
        ///         Client  4.0.30319
        ///         Full  4.0.30319
        /// </summary>
        private static void GetVersionFromRegistry()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                if (ndpKey != null)
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                    {
                        if (versionKeyName.StartsWith("v"))
                        {

                            RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                            if (versionKey != null)
                            {
                                string name = (string)versionKey.GetValue("Version", "");
                                string sp = versionKey.GetValue("SP", "").ToString();
                                string install = versionKey.GetValue("Install", "").ToString();
                                if (install == "") //no install info, ust be later
                                    Console.WriteLine(versionKeyName + "  " + name);
                                else
                                {
                                    if (sp != "" && install == "1")
                                    {
                                        Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                                    }

                                }
                                if (name != "")
                                {
                                    continue;
                                }
                                foreach (string subKeyName in versionKey.GetSubKeyNames())
                                {
                                    RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                                    if (subKey != null)
                                    {
                                        name = (string)subKey.GetValue("Version", "");
                                        if (name != "")
                                            sp = subKey.GetValue("SP", "").ToString();
                                        install = subKey.GetValue("Install", "").ToString();
                                    }
                                    if (install == "") //no install info, ust be later
                                        Console.WriteLine(versionKeyName + "  " + name);
                                    else
                                    {
                                        if (sp != "" && install == "1")
                                        {
                                            Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                        }
                                        else if (install == "1")
                                        {
                                            Console.WriteLine("  " + subKeyName + "  " + name);
                                        }

                                    }

                                }
                            }
                        }
                    }
            }

        }

        /// <summary>
        /// The .NET Framework version 4.5.1 is installed
        /// </summary>
        private static void Get45or451FromRegistry()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
               RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey != null)
                {
                    int releaseKey = (int)ndpKey.GetValue("Release");
                    {
                        if (releaseKey == 378389)

                            Console.WriteLine("The .NET Framework version 4.5 is installed");

                        if (releaseKey == 378575)

                            Console.WriteLine("The .NET Framework version 4.5.1 Preview is installed");

                        if (releaseKey == 378681)

                            Console.WriteLine("The .NET Framework version 4.5.1 RC is installed");
                    }
                }
            }
        }
    }
}
