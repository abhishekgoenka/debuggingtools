using System;
using System.IO.IsolatedStorage;
using System.IO;

namespace Silverlight_Patterns_in_Action.Code
{
    /// <summary>
    /// Isolated storage helper class. Provides read and write access for files.
    /// </summary>
    public static class IsolatedStoreHelper
    {
        /// <summary>
        /// Saves data to a given file.
        /// </summary>
        /// <param name="data">The data to be saved.</param>
        /// <param name="path">The path to the file.</param>
        public static void SaveData(string data, string path)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream(path, FileMode.Create, store))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(data);
                        writer.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads data from a given file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The data being loaded.</returns>
        public static string LoadData(string path)
        {
            string data = String.Empty;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(path))
                {
                    using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, store))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            string lineOfData = String.Empty;
                            while ((lineOfData = reader.ReadLine()) != null)
                                data += lineOfData;
                        }
                    }
                }
            }
            return data;
        }
    }
}
