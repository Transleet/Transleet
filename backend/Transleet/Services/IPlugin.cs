namespace Transleet.Services;

public interface IPlugin
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    string GetPlugin();
}

public interface IPluginImporter : IPlugin
{
    public string FileType { get; set; }
}

public interface IPluginExporter : IPlugin
{
    public string FileType { get; set; }
}