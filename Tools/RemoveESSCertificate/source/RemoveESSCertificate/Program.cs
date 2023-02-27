using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class X509store2
{
    public static void Main(string[] args)
    {
        string Version = "1.0";

        Console.WriteLine($"\nVersion:{Version}");

        //Create new X509 store called teststore from the local certificate store.
        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

        try
        {
            store.Open(OpenFlags.ReadWrite);
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n\nERROR: Re-run with 'Run as Administrator' permissions\n\n");
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
            return;
        }
      

        //Create a collection and add two of the certificates.
        X509Certificate2Collection collection = new X509Certificate2Collection();
      
        X509Certificate2Collection storecollection = (X509Certificate2Collection)store.Certificates;
        Console.WriteLine("+++Initial Certificates in Store++++");
        Console.WriteLine("Store location: {0}", store.Location);        
        foreach (X509Certificate2 x509 in storecollection)
        {
            Console.WriteLine("certificate name: {0}", x509.Subject);
            if (x509.Subject.Contains("E=rs.oasishelp@medtronic.com"))
            {
                Console.WriteLine("\n\nRemoving certificate name: {0}\n\n", x509.Subject);
                store.Remove(x509);
            }
        }

        X509Certificate2Collection storecollection3 = (X509Certificate2Collection)store.Certificates;
        Console.WriteLine("\n\n+++++Certificates in Store after removing+++++");
        if (storecollection3.Count == 0)
        {
            Console.WriteLine("Store contains no certificates.");
        }
        else
        {
            foreach (X509Certificate2 x509 in storecollection3)
            {
                if (x509.Subject.Contains("E=rs.oasishelp@medtronic.com"))
                    Console.WriteLine("certificate name: {0}", x509.Subject);
            }
        }

        //Close the store.
        store.Close();
        Console.WriteLine("\n\nPress any key to exit ...");
        Console.ReadKey();
    }
}