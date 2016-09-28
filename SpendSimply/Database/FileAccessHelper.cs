using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace SpendSimply
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = System.IO.Path.Combine(libraryPath, "..", "Library", "Databases");

            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return System.IO.Path.Combine(libFolder, filename);
        }
    }
}

