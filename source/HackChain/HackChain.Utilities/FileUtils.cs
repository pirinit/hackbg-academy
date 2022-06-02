using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Utilities
{
    public class FileUtils
    {
        public static void ReplaceFileStringContent(string filename, string fileContent)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.AppendAllText(filename, fileContent);
        }
    }
}
