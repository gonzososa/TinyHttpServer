# TinyHttpServer

It was create to help us to simulate _http_ responses for when we are testing or debugging some apis due to it's usually hard to adecuate some escenarios using real api management/api gateway implementations

It can be configured thru _conf.json_ file with desired responses, statuscode, content, contentType params.

## build from sources

### Requires
- Window, Linux, Macos platform
- .NET 7

### Build & Execute
```
$ dotnet build

or

$ dotnet run 
```

## ToDo
- Enable respond files (base64 & binary)
