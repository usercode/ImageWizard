using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Locking
{
    /// <summary>
    /// LockEntryRelease
    /// </summary>
    public class LockEntryReadExit : IDisposable
    {
        public LockEntryReadExit(LockEntry entry)
        {
            LockEntry = entry;
        }

        /// <summary>
        /// LockEntry
        /// </summary>
        private LockEntry LockEntry { get; }

        public void Dispose()
        {
            LockEntry.ExitReadLock();
        }
    }
}
