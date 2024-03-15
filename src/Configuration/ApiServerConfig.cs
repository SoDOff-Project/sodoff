namespace sodoff.Configuration;
public class ApiServerConfig {
    public string ResponseURL { get; set; } = string.Empty;

    public string MMOAdress { get; set; } = "127.0.0.1";
    public int MMOPort { get; set; } = 9933;
    public uint MMOSupportMinVersion { get; set; } = 0;

    public DbProviders DbProvider { get; set; } = DbProviders.SQLite;
    public string DbPath { get; set; } = string.Empty;
    public string DbConnection { get; set; } = string.Empty;
}

public enum DbProviders {
    SQLite, PostgreSQL, MySQL
}
