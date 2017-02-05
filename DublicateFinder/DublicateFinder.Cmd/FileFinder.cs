using System;
using System.Collections.Generic;
using System.IO;

namespace DublicateFinder.Cmd
{
    internal sealed class FileFinder
    {
        public String[] GetAllFileNames(String root)
        {
            Queue<String> roots = new Queue<String>();
            List<String> subdirectories = new List<String>();
            roots.Enqueue(root);
            String currentDirectory = null;

            while (roots.Count > 0)
            {
                currentDirectory = roots.Dequeue();

                subdirectories.Add(currentDirectory);

                String[] subDirectories = Directory.GetDirectories(currentDirectory);

                for (int i = 0; i < subDirectories.Length; i++)
                {
                    roots.Enqueue(subDirectories[i]);
                }
            }

            List<String> fileNames = new List<String>();

            for (int i = 0; i < subdirectories.Count; i++)
            {
                currentDirectory = subdirectories[i];
                String[] filesFromCurrentDirectory = Directory.GetFiles(currentDirectory);

                for (int j = 0; j < filesFromCurrentDirectory.Length; j++)
                {
                    fileNames.Add(filesFromCurrentDirectory[j]);
                }
            }

            return fileNames.ToArray();
        }
    }
}
