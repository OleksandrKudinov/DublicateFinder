using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            Int32 minSize = 1;
            Int32 maxSize = 1000;

            FileFinder provider = new FileFinder(new DebugOutput());

            FileInfo[] files = provider.GetAllFiles(root)
                .Where(file => file.Length >= minSize && file.Length <= maxSize)
                .Take(100)
                .ToArray();

            foreach(FileInfo file in files)
            {
                Console.WriteLine(file.FullName);
            }
        }
    }
}
