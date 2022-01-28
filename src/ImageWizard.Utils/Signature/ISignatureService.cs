using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils
{
    /// <summary>
    /// ISignatureService
    /// </summary>
    public interface ISignatureService
    {
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        string Encrypt(string key, string input);
    }
}
