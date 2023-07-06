namespace TinyHttpServer.Models;

public class Response {
    public int statusCode { get; set; }
    public string contentType { get; set; }
    public string content { get; set; }
}