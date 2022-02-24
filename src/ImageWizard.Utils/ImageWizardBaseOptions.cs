// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils
{
    public abstract class ImageWizardBaseOptions
    {
        public ImageWizardBaseOptions()
        {
            _key = string.Empty;
            _keyDecoded = Array.Empty<byte>();
        }

        private string _key;
        private byte[] _keyDecoded;

        /// <summary>
        /// Key encoded in Base64Url
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;

                    _keyDecoded = WebEncoders.Base64UrlDecode(_key);
                }
            }
        }

        /// <summary>
        /// GetKeyInBytes
        /// </summary>
        /// <returns></returns>
        public byte[] KeyInBytes => _keyDecoded;
    }
}
