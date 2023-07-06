using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TinyHttpServer.Models;

namespace TinyHttpServer.Handlers;

public class Handlers {
    private readonly List<Endpoint> _endpoints;
    private Dictionary<string, Handler> handlers;

    public Handlers (List<Endpoint> endpoints) {
        _endpoints = endpoints;
        handlers = new Dictionary<string, Handler> ();
    }

    public Handlers Build () {
        Regex regex = new Regex (@"\{[a-zA-Z0-9]+\}", RegexOptions.Compiled);
        string pattern = "[a-zA-Z0-9]+"; 

        handlers = _endpoints.Select (endpoint => {
            var templatePattern = regex.Replace (endpoint.path, pattern); //looking for parameters {param}
            if (templatePattern.Last () == '/') templatePattern = templatePattern[..^1]; // last '/' is optional
            templatePattern += "[\\/]{0,1}$";

            return new Tuple<string, Handler>(templatePattern, new Handler (endpoint, _action));
        }).ToDictionary (k => k.Item1, v => v.Item2);
        
        return this;
    }

    public void Match (string path, string method, HttpListenerResponse response) {
        bool found = false;

        foreach (var keyValue in handlers) {
            Regex regex = new Regex (keyValue.Key);
            var match = regex.Match (path);

            if (match.Success) {
                found = true;
                var handler = keyValue.Value;
                var action = handler.Action;
                var endpoint = handler.Endpoint;
                
                action (method, endpoint, response); //call to action
            }
        }

        if (!found) {
            _notFound (response); //notfound as response
        }
    }

   // action to execute
    private readonly Action<string, Endpoint, HttpListenerResponse> _action = async (method, endpoint, response) => {
        if (method.Trim().ToLower () != endpoint.httpMethod.Trim().ToLower()) {
            response.StatusCode = (int) HttpStatusCode.BadRequest;
            await response.OutputStream.WriteAsync (Array.Empty<byte>());
            response.Close ();

            return;
        }

        response.ContentType = endpoint.response.contentType;
        response.StatusCode = endpoint.response.statusCode;
        
        var buffer = Encoding.UTF8.GetBytes (endpoint.response.content);
        var cancellationToken = new CancellationTokenSource ();
        cancellationToken.CancelAfter (TimeSpan.FromSeconds (5));

        await response.OutputStream.WriteAsync (buffer, cancellationToken.Token);
        response.OutputStream.Close ();
    };

    private readonly Action<HttpListenerResponse> _notFound = async (response) => {
        response.StatusCode = (int) HttpStatusCode.NotFound;
        await response.OutputStream.WriteAsync (Array.Empty<byte>());
        response.Close ();
    };

    private class Handler {

        public Handler (Endpoint endpoint, Action<string, Endpoint, HttpListenerResponse> action) {
            Endpoint = endpoint;
            Action = action;
        }
        public Endpoint Endpoint { get; set; }
        public Action<string, Endpoint, HttpListenerResponse> Action { get; set; }

    }
}

