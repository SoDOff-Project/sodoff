namespace sodoff.Configuration;
public class AssetServerConfig {
    public bool Enabled { get; set; } = false;
    public string ListenIP { get; set; } = string.Empty;
    public int Port { get; set; } = 5001;
    public string URLPrefix { get; set; } = string.Empty;
    public AssetServerMode Mode { get; set; }
    public string ProviderURL { get; set; } = string.Empty;
    public string AutoEncryptRegexp { get; set; } = string.Empty;
    public string AutoEncryptKey { get; set; } = string.Empty;
    public bool SubstituteMissingLocalAssets { get; set; } = false;
    public bool UseCache { get; set; } = true;
}

public enum AssetServerMode {
    None, Partial, Full
}
