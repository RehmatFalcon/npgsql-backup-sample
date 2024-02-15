namespace NpgSqlBackup;

public class ImageDirectoryProvider
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public ImageDirectoryProvider(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public string GetPath()
    {
        // var path = _configuration["IMAGE_DIR"] ?? "my_dir";
        // Directory.CreateDirectory(path);
        // return Path.Combine(_environment.WebRootPath, path);

        var path = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(path);
        return path;
    }
}