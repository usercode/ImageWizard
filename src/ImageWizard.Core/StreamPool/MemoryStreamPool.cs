using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public class MemoryStreamPool : IStreamPool
    {
        public Stream GetStream()
        {
            return new MemoryStream();
        }
    }
}
