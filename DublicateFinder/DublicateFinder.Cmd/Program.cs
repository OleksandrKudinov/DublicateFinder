using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        static volatile Int32 count = 0;
        static volatile Int32 total = 0;

        static void Main(string[] args)
        {
            String root = @"D:\test1";
            Int32 minSize = 1;
            Int32 maxSize = Int32.MaxValue;

            FileFinder provider = new FileFinder(new DebugOutput());

            Func<FileInfo, Boolean> filter = file =>
            {
                Boolean isOk = file.Length >= minSize && file.Length <= maxSize;
                total++;
                if (isOk)
                {
                    count++;
                }
                return isOk;
            };

            // like progress bar
            CancellationTokenSource source = new CancellationTokenSource();
            Task.Run(async () =>
            {
                var token = source.Token;

                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine($"{nameof(total)} : {total}, {nameof(count)} : {count}");
                    await Task.Delay(100);
                }
                Console.WriteLine($"{nameof(total)} : {total}, {nameof(count)} : {count}");
                Console.WriteLine("canceled or stopped");
            });

            FileInfo[] files = provider.GetAllFiles(root)
                .Where(filter)
                .Take(1000)
                .ToArray();

            source.Cancel();

            IEnumerable<FileWrapper> wrapFiles = files.Select(fileinfo => new FileWrapper(fileinfo));

            IEnumerable<IEnumerator<DataBlock>> blockEnumerators = wrapFiles
                .Select(fileWrapper => fileWrapper.GetEnumerator())
                .ToArray();

            // Read first DataBlock for each file 
            if (!blockEnumerators.All(x => x.MoveNext()))
            {
                return;
            }

            foreach (var block in blockEnumerators.OrderBy(x => x.Current))
            {
                Console.WriteLine(block.Current.Buffer.Length);
            }

            // group by size
            foreach (var group in files.GroupBy(x => x.Length).Where(x => x.Count() > 1))
            {
                Console.WriteLine($"{group.Key} {group.Count()}");
            }

            Console.ReadLine();
        }
    }
}
