using System.Text.Json;
using TinyHttpServer.Models;

namespace TinyHttpServer;

public class Startup {

    public static Configuration GetConfiguration  {
        get {
            if (!Path.Exists ("conf.json")) throw new FileNotFoundException ("Configuration fie is missing");

            var reader = new StreamReader (new FileStream ("conf.json", FileMode.Open));
            var content = reader.ReadToEnd ();
            var configuration = JsonSerializer.Deserialize<Configuration>(content);

            return configuration;
        }
    }

}