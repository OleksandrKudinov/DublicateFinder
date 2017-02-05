using System;
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

            String[] files = provider.GetAllFileNames(root);

            Console.WriteLine("Files:");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
        }
    }
}
