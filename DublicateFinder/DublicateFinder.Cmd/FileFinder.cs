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

        public IEnumerable<String> GetAllFileNames(String root)
        {
            IEnumerable<String> subdirectories = GetAllSubDirectories(root);
            String[] filesFromCurrentDirectory = null;
            foreach (String currentDirectory in subdirectories)
            {
                try
                {
                    filesFromCurrentDirectory = Directory.GetFiles(currentDirectory);
                }
                catch (Exception exception)
                {
                    _progress?.Report($"{exception.Message}");
                    filesFromCurrentDirectory = null;
                }

                if ((filesFromCurrentDirectory?.Any()) == true)
                {
                    foreach (String fileName in filesFromCurrentDirectory)
                    {
                        yield return fileName;
                    }
                }
            }
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
                    _progress?.Report($"{exception.Message}");
                }

                yield return currentDirectory;
            }
        }

        private readonly IProgress<String> _progress;
    }
}
