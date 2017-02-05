using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            IEnumerable<String> fileNames = provider.GetAllFileNames(root);

            Console.WriteLine("Files:");
            foreach (String fileName in fileNames)
            {
                Console.WriteLine(fileName);
            }
        }
    }
}
