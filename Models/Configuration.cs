namespace TinyHttpServer.Models;

public class Configuration {
    public string basePath { get; set; }
    public int port { get; set; }
    public List<Endpoint> endpoints { get; set; }
}
