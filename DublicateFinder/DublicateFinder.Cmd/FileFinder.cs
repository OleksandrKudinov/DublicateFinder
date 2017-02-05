using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            IEnumerable<String> subdirectories = GetAllSubDirectories(root);

            List<String> fileNames = new List<String>();

            foreach(String currentDirectory in subdirectories)
            {     
                String[] filesFromCurrentDirectory = Directory.GetFiles(currentDirectory);

                fileNames.AddRange(filesFromCurrentDirectory);
            }

            return fileNames.ToArray();
        }

        private IEnumerable<String> GetAllSubDirectories(String root)
        {
            Queue<String> roots = new Queue<String>();

            roots.Enqueue(root);
            String currentDirectory = null;

            while (roots.Count > 0)
            {
                currentDirectory = roots.Dequeue();

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

                yield return currentDirectory;
            }
        }

        private readonly IProgress<string> _progress;
    }
}
