﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.MongoDB.ImageCaches
{
    /// <summary>
    /// MongoDBCacheOptions
    /// </summary>
    public class MongoDBCacheOptions
    {
        public MongoDBCacheOptions()
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
