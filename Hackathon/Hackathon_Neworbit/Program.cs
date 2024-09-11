using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = new HostApplicationBuilder();
builder.Configuration.AddUserSecrets("c0515972-21aa-4f00-af01-2e17a542c4e1");
var application = builder.Build();

Console.WriteLine("BOREDOM ANNIHIALATOR (v1)\n");

Console.WriteLine("Boredom annihilation in progress...");

var configuration = application.Services.GetRequiredService<IConfiguration>();

var kernel = Kernel.
    CreateBuilder().AddAzureOpenAIChatCompletion("gpt4o", "https://boredom-euw-ai.openai.azure.com/", configuration.GetValue<string>("openaiKey")!)
    .Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.Add(new ChatMessageContent(AuthorRole.User, "How can I annihilate my boredom?"));
var completions = await chatCompletionService.GetChatMessageContentsAsync(history);
Console.WriteLine(completions.Last());
