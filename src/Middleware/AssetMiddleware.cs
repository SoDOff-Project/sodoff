using Microsoft.Extensions.Options;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using sodoff.Configuration;
using sodoff.Util;

namespace sodoff.Middleware;
public class AssetMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<AssetServerConfig> config;
    private readonly Regex autoEncryptRegexp;

    public AssetMiddleware(RequestDelegate next, IOptions<AssetServerConfig> config)
    {
        _next = next;
        this.config = config;
        if (config.Value.AutoEncryptRegexp != "")
            autoEncryptRegexp = new Regex(config.Value.AutoEncryptRegexp);
        else
            autoEncryptRegexp = null;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Connection.LocalPort == config.Value.Port)
            await GetAssetAsync(context);
        else
            await _next(context);
    }

    private async Task GetAssetAsync(HttpContext context)
    {
        string path = context.Request.Path;

        if (path is null || !string.IsNullOrEmpty(config.Value.URLPrefix) && !path.StartsWith("/" + config.Value.URLPrefix) || config.Value.Mode == AssetServerMode.None) {
            context.Response.StatusCode = 400;
            return;
        }

        string assetPath = path.Remove(0, config.Value.URLPrefix.Length + 1);
        string localPath = GetLocalPath("assets/" + assetPath);

        if (localPath == string.Empty && config.Value.Mode == AssetServerMode.Partial && config.Value.UseCache)
            localPath = GetLocalPath("assets-cache/" + assetPath);

        if (localPath == string.Empty) {
            if (config.Value.Mode == AssetServerMode.Partial)
                await GetRemoteAsset(context, assetPath);
            else
                context.Response.StatusCode = 404;
        }
        else {
            context.Response.Headers["Content-Type"] = "application/octet-stream";
            bool needEncrypt = autoEncryptRegexp != null && autoEncryptRegexp.IsMatch(localPath);
            if (needEncrypt) {
                await context.Response.WriteAsync(
                    TripleDES.EncryptASCII(
                        System.IO.File.ReadAllText(localPath),
                        config.Value.AutoEncryptKey
                    )
                );
            } else {
                await context.Response.SendFileAsync(Path.GetFullPath(localPath));
            }
        }
    }

    private async Task GetRemoteAsset(HttpContext context, string path)
    {
        HttpClient client = new HttpClient();
        string filePath = Path.GetFullPath("assets-cache/" + path);
        string filePathTmp = filePath + Path.GetRandomFileName().Substring(0, 8);
        try {
            using (var response = await client.GetAsync(
                config.Value.ProviderURL + path,
                HttpCompletionOption.ResponseHeadersRead
            )) {
                if (response.IsSuccessStatusCode) {
                    if (response.Content.Headers.ContentType?.MediaType != null)
                        context.Response.Headers["Content-Type"] =  response.Content.Headers.ContentType?.MediaType;

                    if (response.Content.Headers.ContentLength != null)
                        context.Response.Headers["Content-Length"] = response.Content.Headers.ContentLength.ToString();

                    using (var inputStream = await response.Content.ReadAsStreamAsync()) {
                        if (config.Value.UseCache) {
                            string dirPath = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(dirPath)) {
                                Directory.CreateDirectory(dirPath);
                            }

                            // copy data retrieved from upstream server to file and to response for game client
                            using (var fileStream = File.Open(filePathTmp, FileMode.Create)) {
                                // read response from upstream server
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                                    // write to temporary file
                                    var task1 = fileStream.WriteAsync(buffer, 0, bytesRead);
                                    // send to client
                                    var task2 = context.Response.Body.WriteAsync(buffer, 0, bytesRead);
                                    // wait for finish both writes
                                    await Task.WhenAll(task1, task2);
                                }
                            }

                            // after successfully write data to temporary file, rename it to proper asset filename
                            File.Move(filePathTmp, filePath);
                        } else {
                            await inputStream.CopyToAsync(context.Response.Body);
                        }
                    }
                } else {
                    context.Response.StatusCode = 404;
                }
            }
        }
        catch (Exception) {
            if (File.Exists(filePathTmp))
                File.Delete(filePathTmp);
            if (!context.Response.HasStarted)
                context.Response.StatusCode = 502;
        }
    }

    private string GetLocalPath(string path)
    {
        if (File.Exists(path)) return path;

        string[] qualityTiers = { "/High/", "/Mid/", "/Low/" };

        if (config.Value.SubstituteMissingLocalAssets)
        {
            foreach (var tier in qualityTiers)
            {
                if (path.Contains(tier))
                {
                    foreach (var otherTier in qualityTiers)
                    {
                        if (otherTier != tier)
                        {
                            string otherPath = path.Replace(tier, otherTier);
                            if (File.Exists(otherPath)) return otherPath;
                        }
                    }
                }
            }
        }

        return string.Empty;
    }
}
