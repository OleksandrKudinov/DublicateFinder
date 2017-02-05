using System;
using System.Collections.Generic;
using System.IO;

namespace DublicateFinder.Cmd
{
    internal sealed class FileFinder
    {
        public String[] GetAllFileNames(String root)
        {
            String[] subdirectories = GetAllSubDirectories(root);
            List<String> fileNames = new List<String>();

            String currentDirectory = null;

            for (int i = 0; i < subdirectories.Length; i++)
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

        private String[] GetAllSubDirectories(String root)
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

            return subdirectories.ToArray();
        }
    }
}
