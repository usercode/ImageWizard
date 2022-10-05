// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.MongoDB;

/// <summary>
/// MongoDBCacheOptions
/// </summary>
public class MongoDBCacheOptions
{
    public MongoDBCacheOptions()
    {
        Hostname = "localhost";
        Database = "ImageWizard";
        Username = string.Empty;
        Password = string.Empty;
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
