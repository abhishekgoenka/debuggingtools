using System;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

class CertSelect
{

    static void Main()
    {
        //Searching in current user
        Console.WriteLine("Certificates from current user");
        ListCert(StoreLocation.CurrentUser);

        //Searching in Local Machine
        Console.WriteLine("Certificates from local machine");
        ListCert(StoreLocation.LocalMachine);

        //wait till user closess the screen
        Console.ReadKey();
    }

    private static void ListCert(StoreLocation location)
    {
        X509Store store = new X509Store(location);
        store.Open(OpenFlags.ReadOnly);
        foreach (X509Certificate2 x509Certificate2 in store.Certificates)
        {
            X500DistinguishedName dname = new X500DistinguishedName(Convert.ToString(x509Certificate2.SubjectName.Name), X500DistinguishedNameFlags.Reversed);
            String cn = PharseDN(x509Certificate2.SubjectName.Name);
            Console.WriteLine("SubjectName : {0}, Thumpprint : {1}, CN : {2}", x509Certificate2.SubjectName.Name, x509Certificate2.Thumbprint, cn);
            x509Certificate2.Reset();
        }    
        store.Close();


        FindCert(location);
    }

    /// <summary>
    /// Parse common name from distinguished  
    /// </summary>
    /// <param name="dn">Distinguished Name</param>
    /// <returns>Common Name</returns>
    private static String PharseDN(String dn)
    {
        //We have only 1 value in DN. Example : CN = xxxxx
        if (!dn.Contains(",")) return dn;

        String[] rdn = dn.Split(',');
        foreach (String item in rdn)
        {
            if (item.StartsWith("CN", StringComparison.OrdinalIgnoreCase)) return item;
        }
        return String.Empty;
    }

    private static void FindCert(StoreLocation location)
    {
        try
        {
            Console.WriteLine("Searching certificate in : {0}", location);
            X509Store store = new X509Store(location);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection) store.Certificates;
            X509Certificate2Collection fcollection =
                (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectName, "CTLLabCert.pfx", false);
            Console.WriteLine("Number of certificates: {0}{1}", fcollection.Count, Environment.NewLine);

            //fcollection =
            //   collection.Find(X509FindType.FindByThumbprint, "F5 4A D4 77 B0 3A 6C 65 CC 83 E3 50 43 44 F8 8D F3 CC 68 61", true);

            foreach (X509Certificate2 x509 in fcollection)
            {
                byte[] rawdata = x509.RawData;
                Console.WriteLine("Friendly Name: {0}{1}", x509.FriendlyName, Environment.NewLine);
                Console.WriteLine("Simple Name: {0}{1}", x509.GetNameInfo(X509NameType.SimpleName, true),
                    Environment.NewLine);
            }
            store.Close();
        }

        catch (CryptographicException)
        {
            Console.WriteLine("Information could not be written out for this certificate.");
        }
    }
}