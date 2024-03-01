namespace sodoff.Configuration;
public class ApiServerConfig {
    public string ResponseURL { get; set; } = string.Empty;
    
    public DbProviders DbProvider { get; set; } = DbProviders.SQLite;
    public string DbPath { get; set; } = string.Empty;
    public string DbConnection { get; set; } = string.Empty;
}

public enum DbProviders {
    SQLite, PostgreSQL, MySQL
}
