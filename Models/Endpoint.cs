namespace TinyHttpServer.Models;

public class Endpoint {
    public string path { get; set; }
    public string httpMethod { get; set; }
    public Response response { get; set; }
}
