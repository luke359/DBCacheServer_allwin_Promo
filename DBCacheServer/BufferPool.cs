using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public class BufferPool
    {
        private readonly ConcurrentQueue<byte[]> _pool = new();
        private readonly int _bufferSize;

        public BufferPool(int count, int bufferSize)
        {
            _bufferSize = bufferSize;
            for (int i = 0; i < count; i++)
                _pool.Enqueue(new byte[bufferSize]);
        }

        public byte[] Rent() => _pool.TryDequeue(out var buf) ? buf : new byte[_bufferSize];

        public void Return(byte[] buffer)
        {
            _pool.Enqueue(buffer);
        }
    }
}
