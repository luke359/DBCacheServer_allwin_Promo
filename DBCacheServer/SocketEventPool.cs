using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DBCacheServer
{
    public class SocketEventPool
    {
        Stack<SocketAsyncEventArgs> m_pool;

        /// <summary>
        /// 目前在池中的實例(參考比對), 用來擋掉重複 Push。
        /// 同一個 SocketAsyncEventArgs 若被放進池子兩次, 會有兩條連線同時拿到它,
        /// 第二條呼叫 ReceiveAsync 時就會拋
        /// "An asynchronous socket operation is already in progress using this SocketAsyncEventArgs instance."
        /// </summary>
        HashSet<SocketAsyncEventArgs> m_inPool;

        // Initializes the object pool to the specified size
        //
        // The "capacity" parameter is the maximum number of
        // SocketAsyncEventArgs objects the pool can hold
        public SocketEventPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
            m_inPool = new HashSet<SocketAsyncEventArgs>();
        }

        /// <summary>
        /// 將SocketAsyncEventArgs放入池堆疊
        /// </summary>
        /// <param name="item"></param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }

            lock (m_pool)
            {
                if (!m_inPool.Add(item))
                {
                    // 已經在池子裡了, 代表上層有重複釋放的路徑。忽略這次 Push 以免池子被污染。
                    MyConsole.WriteLine("SocketEventPool.Push: 偵測到重複歸還的 SocketAsyncEventArgs, 已忽略");
                    return;
                }

                m_pool.Push(item);
            }
        }

        /// <summary>
        /// 原子性地宣告「由呼叫端負責歸還這個實例」。
        /// 若它已經在池中(或已有另一條路徑正在歸還), 回傳 false, 呼叫端必須立刻放棄釋放流程。
        /// 這是連線關閉流程的冪等保護: 重複釋放會讓號誌被多 Release、
        /// 同一個 SocketAsyncEventArgs 被重複放回池子, 造成兩條連線共用同一個實例。
        /// 成功後必須配對呼叫 EndRelease。
        /// </summary>
        public bool TryBeginRelease(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                return false;
            }

            lock (m_pool)
            {
                return m_inPool.Add(item);
            }
        }

        /// <summary>
        /// 完成歸還, 讓實例真正能被下一條連線取用。必須在 TryBeginRelease 成功之後呼叫。
        /// </summary>
        public void EndRelease(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                return;
            }

            lock (m_pool)
            {
                // 只有先前成功 TryBeginRelease 的實例才允許進入堆疊
                if (m_inPool.Contains(item))
                {
                    m_pool.Push(item);
                }
            }
        }

        /// <summary>
        /// 將SocketAsyncEventArgs從池堆疊取出
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                SocketAsyncEventArgs item = m_pool.Pop();
                m_inPool.Remove(item);
                return item;
            }
        }

        public SocketAsyncEventArgs TryPop()
        {
            lock (m_pool)
            {
                if (m_pool.Count == 0)
                {
                    return null;
                }

                SocketAsyncEventArgs item = m_pool.Pop();
                m_inPool.Remove(item);
                return item;
            }
        }

        // The number of SocketAsyncEventArgs instances in the pool
        public int Count
        {
            get
            {
                lock (m_pool)
                {
                    return m_pool.Count;
                }
            }
        }

        public void Clear()
        {
            lock (m_pool)
            {
                m_pool.Clear();
                m_inPool.Clear();
            }
        }
    }
}
