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
            String[] subdirectories = GetAllSubDirectories(root).ToArray();
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

                //https://msdn.microsoft.com/uk-ua/library/9k7k7cf0.aspx
                //https://habrahabr.ru/post/136828/
                //https://msdn.microsoft.com/ru-ru/library/system.collections.ienumerable(v=vs.110).aspx
                //https://msdn.microsoft.com/ru-ru/library/system.collections.ienumerator(v=vs.110).aspx
                yield return currentDirectory;
            }
        }

        private readonly IProgress<string> _progress;
    }
}
