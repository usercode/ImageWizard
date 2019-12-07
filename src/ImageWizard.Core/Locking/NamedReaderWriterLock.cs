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

        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private ReaderWriterLockSlim GetLock(string name)
        {
            return _lockItems.GetOrAdd(name, s => new ReaderWriterLockSlim());
        }

        /// <summary>
        /// GetLock
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ReaderWriterLockSlim GetReadLock(string name)
        {
            _lock.EnterReadLock();

            try
            {
                var r = GetLock(name);

                r.EnterReadLock();

                return r;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private ReaderWriterLockSlim GetWriteLock(string name)
        {
            _lock.EnterReadLock();

            try
            {
                var r = GetLock(name);

                r.EnterWriteLock();

                return r;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void RunWithReadLock(string name, Action body)
        {
            var rwLock = GetReadLock(name);

            try
            {
                body();
            }
            finally
            {
                rwLock.ExitReadLock();

                RemoveLock(name);
            }
        }

        public void RunWithWriteLock(string name, Action body)
        {
            var rwLock = GetWriteLock(name);

            try
            {
                body();
            }
            finally
            {
                rwLock.ExitWriteLock();

                RemoveLock(name);
            }
        }

        public void RemoveLock(string name)
        {
            _lock.EnterWriteLock();

            try
            {
                if (_lockItems.TryGetValue(name, out ReaderWriterLockSlim value))
                {
                    //is lock currently used?
                    if (value.CurrentReadCount == 0
                        && value.WaitingReadCount == 0
                        && value.WaitingUpgradeCount == 0
                        && value.WaitingWriteCount == 0)
                    {
                        _lockItems.TryRemove(name, out ReaderWriterLockSlim o);

                        value.Dispose();                        
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
