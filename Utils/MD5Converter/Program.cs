using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MD5Converter
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.Write("Enter string : ");
            String originalString = Console.ReadLine();

            if (!String.IsNullOrEmpty(originalString))
            {
                String md5 = MD5Hash(originalString);
                Console.WriteLine("MD5 String : {0}", md5);
                Clipboard.SetText(md5);
            }

            //halt main thread
            Console.ReadKey();
        }

        static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        } 
    }
}
