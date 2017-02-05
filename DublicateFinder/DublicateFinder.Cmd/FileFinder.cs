using System;
using System.Collections.Generic;
using System.IO;

namespace DublicateFinder.Cmd
{
    internal sealed class FileFinder
    {
        public FileFinder(IProgress<String> progress = null)
        {
            _progress = progress;
        }

        public String[] GetAllFileNames(String root)
        {
            String[] subdirectories = GetAllSubDirectories(root);
            List<String> fileNames = new List<String>();

            String currentDirectory = null;

            for (int i = 0; i < subdirectories.Length; i++)
            {
                currentDirectory = subdirectories[i];
                String[] filesFromCurrentDirectory = Directory.GetFiles(currentDirectory);

                // To investigate https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,e569d850a66a1771,references
                fileNames.AddRange(filesFromCurrentDirectory);
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

                try
                {
                    String[] subDirectories = Directory.GetDirectories(currentDirectory);

                    for (int i = 0; i < subDirectories.Length; i++)
                    {
                        roots.Enqueue(subDirectories[i]);
                    }
                }
                catch (UnauthorizedAccessException exception)
                {
                    _progress?.Report(exception.Message);
                }
            }

            return subdirectories.ToArray();
        }

        private readonly IProgress<string> _progress;
    }
}
