using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DublicateFinder.Cmd
{
    class DebugOutput : IProgress<String>
    {
        public void Report(String message)
        {
            Debug.WriteLine(message);
        }
    } 

    class Program
    {
        static void Main(string[] args)
        {
            String root = @"D:\";
            FileFinder provider = new FileFinder(new DebugOutput());

            IEnumerable<FileInfo> files = provider.GetAllFiles(root);

            Console.WriteLine("Files:");
            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.Length);
            }
        }
    }
}
