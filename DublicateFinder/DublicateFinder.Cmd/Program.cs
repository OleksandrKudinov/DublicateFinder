using System;

namespace DublicateFinder.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            String root = @"D:\";
            FileFinder provider = new FileFinder();

            String[] files = provider.GetAllFileNames(root);

            Console.WriteLine("Files:");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
        }
    }
}
