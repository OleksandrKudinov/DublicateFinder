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

        public IEnumerable<FileInfo> GetAllFiles(String root)
        {
            IEnumerable<String> fileNames = GetAllFileNames(root);
            FileInfo fileInfo = null;

            foreach (String fileName in fileNames)
            {
                try
                {
                    fileInfo = new FileInfo(fileName);
                }
                catch (PathTooLongException exception)
                {
                    /*
                     * Probably you can try to use .NET 4.6.2 to avoid this issue [1], 
                     * but in this case you will have troubles at least with FileInfo. 
                     * Especially when you try to read Length propery of FileInfo, 
                     * you will have FileNotFound exception.
                     * 
                     * [1] https://blogs.msdn.microsoft.com/jeremykuhne/2016/07/30/net-4-6-2-and-long-paths-on-windows-10/
                     * [2] https://blogs.msdn.microsoft.com/bclteam/2007/02/13/long-paths-in-net-part-1-of-3-kim-hamilton/
                     * 
                     * I think it will be probably a good choice to send message 
                     * about this exception to UI just for notification about excluding 
                     * this file from processing.
                     */
                    _progress.Report($"{exception.Message}");
                    continue;
                }

                yield return fileInfo;
            }
        }

        public IEnumerable<String> GetAllFileNames(String root)
        {
            IEnumerable<String> subdirectories = GetAllSubDirectories(root);

            foreach (String currentDirectory in subdirectories)
            {
                foreach (String filename in ExtractFilesFromDirectory(currentDirectory))
                {
                    yield return filename;
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

        private IEnumerable<String> ExtractFilesFromDirectory(String directory)
        {
            String[] filesFromCurrentDirectory = null;
            try
            {
                filesFromCurrentDirectory = Directory.GetFiles(directory);
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

        private readonly IProgress<String> _progress;
    }
}
