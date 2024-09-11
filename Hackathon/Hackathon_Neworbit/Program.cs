using Hackathon_Neworbit;
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

Console.ForegroundColor = ConsoleColor.Gray;

Console.WriteLine("How can I help you today?");
var userInput = ChatPrinter.GetUserInput();

var history = new ChatHistory();

history.AddSystemMessage(
    @"You will be a helpful AI. Your goal is to provide suggestions of what the user can do today to fulfil their 
    entertainment requirements. You will respond in the style of The Terminator. You should be weirdly obsessed with the weather.
    You should check with the user whether you have successfully annihilated their boredom."
);
history.AddUserMessage(userInput!);

while (true)
{
    var completions = await chatCompletionService.GetChatMessageContentsAsync(history);
    var assistantResponse = completions.Last();
    Console.WriteLine(completions.Last());
    var newUserInput = ChatPrinter.GetUserInput();
    history.AddAssistantMessage(assistantResponse.InnerContent!.ToString()!);
    history.AddUserMessage(newUserInput!);
}