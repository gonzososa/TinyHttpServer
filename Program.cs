using System.Net;
using TinyHttpServer;
using TinyHttpServer.Handlers;

var configuration = Startup.GetConfiguration;
var handlers = new Handlers (configuration.endpoints).Build ();

using var listener = new HttpListener ();
listener.Prefixes.Add ($"{configuration.basePath}:{configuration.port}/");
listener.Start ();

while (true) {
    var context = listener.GetContext ();
    
    HttpListenerRequest request = context.Request;
    Console.WriteLine ($"Received request for path: {request.RawUrl}");
    
    handlers.Match (request.RawUrl, request.HttpMethod, context.Response);
}




