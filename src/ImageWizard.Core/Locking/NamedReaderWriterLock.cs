using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ImageWizard.Core.Locking
{
    /// <summary>
    /// NamedReaderWriterLocker
    /// </summary>
    public class NamedReaderWriterLocker
    {
        private readonly ConcurrentDictionary<string, ReaderWriterLockSlim> _lockItems = new ConcurrentDictionary<string, ReaderWriterLockSlim>();

        private ReaderWriterLockSlim GetLock(string name)
        {
            return _lockItems.GetOrAdd(name, s => new ReaderWriterLockSlim());
        }

        /// <summary>
        /// GetLock
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReaderWriterLockSlim GetReadLock(string name)
        {
            lock (this)
            {
                var r = GetLock(name);

                r.EnterReadLock();

                return r;
            }
        }

        public ReaderWriterLockSlim GetWriteLock(string name)
        {
            lock (this)
            {
                var r = GetLock(name);

                r.EnterWriteLock();

                return r;
            }
        }

        public TResult RunWithReadLock<TResult>(string name, Func<TResult> body)
        {
            var rwLock = GetLock(name);
            try
            {
                rwLock.EnterReadLock();
                return body();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public void RunWithReadLock(string name, Action body)
        {
            var rwLock = GetLock(name);
            try
            {
                rwLock.EnterReadLock();
                body();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public TResult RunWithWriteLock<TResult>(string name, Func<TResult> body)
        {
            var rwLock = GetLock(name);
            try
            {
                rwLock.EnterWriteLock();
                return body();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public void RunWithWriteLock(string name, Action body)
        {
            var rwLock = GetLock(name);
            try
            {
                rwLock.EnterWriteLock();
                body();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public void RemoveLock(string name)
        {
            lock (this)
            {
                if (_lockItems.TryGetValue(name, out ReaderWriterLockSlim value))
                {
                    if (value.WaitingReadCount == 0 && value.WaitingUpgradeCount == 0 && value.WaitingWriteCount == 0)
                    {
                        _lockItems.TryRemove(name, out ReaderWriterLockSlim o);
                    }
                }
            }
        }
    }
}
