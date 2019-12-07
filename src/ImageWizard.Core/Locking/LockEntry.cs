using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Core.Locking
{
    /// <summary>
    /// LockEntry
    /// </summary>
    public class LockEntry : IDisposable
    {
        public LockEntry(string key)
        {
            Key = key;

            _syncRoot = new object();
            _currentReaders = 0;
        }

        private object _syncRoot;
        private int _currentReaders;

        private SemaphoreSlim Reader = new SemaphoreSlim(1);
        private SemaphoreSlim Writer = new SemaphoreSlim(1);

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        public async Task<LockEntryReadExit> EnterReadLockAsync()
        {
            await Reader.WaitAsync();

            try
            {
                await Writer.WaitAsync();

                lock (_syncRoot)
                {
                    _currentReaders++;
                }

                Writer.Release();

                return new LockEntryReadExit(this);
            }
            finally
            {
                Writer.Release();
            }
        }

        public void ExitReadLock()
        {
            lock(_syncRoot)
            {
                _currentReaders--;
            }

            Reader.Release();
        }

        public async Task<LockEntryWriteExit> EnterWriteLockAsync()
        {
            await Writer.WaitAsync();

            return new LockEntryWriteExit(this);
        }

        public void ExitWriteLock()
        {
            Writer.Release();
        }

        public void Dispose()
        {
            if (Writer != null)
            {
                Writer.Dispose();
            }
        }
    }
}
