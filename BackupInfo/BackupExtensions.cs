namespace NpgSqlBackup.BackupInfo;

public static class DatabaseInfoExtensions
{
    public static DbInfo GetDbInfo(
        string connectionString)
    {
        string server = "", port = "", username = "", password = "", databaseName = "";
        var parts = connectionString.TrimEnd(';').Split(';');
        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            var key = keyValue[0]?.Trim()?.ToLower();
            var value = keyValue[1]?.Trim();
            switch (key)
            {
                case "server":
                    server = value;
                    break;
                case "port":
                    port = value;
                    break;
                case "username":
                    username = value;
                    break;
                case "password":
                    password = value;
                    break;
                case "database":
                    databaseName = value;
                    break;
            }
        }

        return new DbInfo
            { Server = server, Port = port, Username = username, Password = password, DatabaseName = databaseName };
    }
}

public class DbInfo
{
    public string Server { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string DatabaseName { get; set; }
}