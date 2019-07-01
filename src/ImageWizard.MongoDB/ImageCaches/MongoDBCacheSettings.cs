using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.MongoDB.ImageCaches
{
    /// <summary>
    /// MongoDBCacheSettings
    /// </summary>
    public class MongoDBCacheSettings
    {
        public MongoDBCacheSettings()
        {
            Hostname = "localhost";
            Database = "ImageWizard";
        }

        /// <summary>
        /// Hostname
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
