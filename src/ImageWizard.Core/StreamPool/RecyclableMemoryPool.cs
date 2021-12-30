using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.StreamPooling
{
    public class RecyclableMemoryPool : IStreamPool
    {
        private static readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

        public RecyclableMemoryPool()
        {

        }

        public Stream GetStream()
        {            
            return manager.GetStream();
        }
    }
}
