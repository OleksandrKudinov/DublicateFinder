using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DublicateFinder.Cmd
{
    internal sealed class FileWrapper : IEnumerable<DataBlock>
    {
        public FileWrapper(FileInfo fileinfo)
        {
            _fileInfo = fileinfo;
        }

        private IEnumerable<DataBlock> GetBlocks()
        {
            Int64 leftToRead = _fileInfo.Length;
            Int32 maxSize = DataBlock.MaxSize;
            Int32 currentBlockSize = 0;
            Int32 offset = 0;

            using (var stream = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                Byte[] buffer = new Byte[leftToRead > maxSize ? maxSize : leftToRead];

                while (leftToRead > 0)
                {
                    currentBlockSize = leftToRead > maxSize
                        ? maxSize
                        : (Int32)leftToRead;

                    if (currentBlockSize < buffer.Length)
                    {
                        buffer = new Byte[currentBlockSize];
                    }

                    stream.Read(buffer, offset, currentBlockSize);

                    leftToRead -= currentBlockSize;
                    offset += currentBlockSize;

                    yield return new DataBlock(buffer);
                }
            }
        }

        public IEnumerator<DataBlock> GetEnumerator()
        {
            return GetBlocks().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetBlocks().GetEnumerator();
        }

        private readonly FileInfo _fileInfo;
    }
}
