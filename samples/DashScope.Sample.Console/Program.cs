using DashScope;
using DashScope.Models;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets(typeof(Program).Assembly)
                        .Build();

var client = new DashScopeClient(configuration["DashScope:ApiKey"]);

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User:");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }
    if (userInput == "exit")
    {
        break;
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Bot:");
    var result = await client.GenerationAsync(new CompletionRequest()
    {
        Input =
        {
             Prompt = userInput
        }
    });
    Console.WriteLine(result.Output.Text);
}