using System;

namespace DublicateFinder.Cmd
{
    internal sealed class DataBlock : IComparable<DataBlock>, IComparable
    {
        public static Int32 MaxSize = 4096;

        public DataBlock(Byte[] block)
        {
            _buffer = block;
        }

        public Int32 CompareTo(DataBlock other)
        {
            Byte[] otherBuffer = other._buffer;
            if (otherBuffer.Length != _buffer.Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (Int32 i = 0; i < _buffer.Length; i++)
            {
                if (_buffer[i] != otherBuffer[i])
                {
                    return _buffer[i] > otherBuffer[i] ? 1 : -1;
                }
            }

            return 0;
        }

        public Int32 CompareTo(object obj)
        {
            return CompareTo(obj as DataBlock);
        }

        private readonly byte[] _buffer;
    }
}
