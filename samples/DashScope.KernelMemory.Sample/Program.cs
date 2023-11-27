
using DashScope;
using DashScope.KernelMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;

var config = new ConfigurationBuilder()
    .AddUserSecrets(typeof(Program).Assembly)
    .Build();

var client = new DashScopeClient(config["DashScope:ApiKey"]!);

var memory = new KernelMemoryBuilder()
                .WithDefaultDashScopes(client)
                .Build();

await memory.ImportDocumentAsync("sample-SK-Readme.pdf");

var question = "What's Semantic Kernel?";

Console.WriteLine($"\n\nQuestion: {question}");

var answer = await memory.AskAsync(question);

Console.WriteLine($"\nAnswer: {answer.Result}");

Console.WriteLine("\n\n  Sources:\n");

foreach (var x in answer.RelevantSources)
{
    Console.WriteLine($"  - {x.SourceName}  - {x.Link} [{x.Partitions.First().LastUpdate:D}]");
}
