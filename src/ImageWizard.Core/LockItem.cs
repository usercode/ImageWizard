using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ImageWizard.Core
{
    class LockItem
    {
        private IDictionary<string, string> Entries { get; }

        public LockItem()
        {
            Entries = new ConcurrentDictionary<string, string>();
        }

        public void ReadLock(string key)
        {
            ReaderWriterLockSlim read = new ReaderWriterLockSlim();
            
            if (Entries.ContainsKey(key))
            {
                
            }
        }
    }
}
