using Azure.Core.Cryptography;
using Hackathon_Neworbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

const string SuccessMessage = "***BOREDOM ANNIHILATED***";

var builder = new HostApplicationBuilder();
builder.Configuration.AddUserSecrets("c0515972-21aa-4f00-af01-2e17a542c4e1");
var application = builder.Build();

Console.WriteLine("BOREDOM ANNIHIALATOR (v1)\n");

Console.WriteLine("Boredom annihilation in progress...");

var configuration = application.Services.GetRequiredService<IConfiguration>();
var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion("gpt4o", "https://boredom-euw-ai.openai.azure.com/", configuration.GetValue<string>("openaiKey")!);
kernelBuilder.Plugins.Services.AddHttpClient();
kernelBuilder.Plugins.Services.AddSingleton<IConfiguration>(configuration);
kernelBuilder.Plugins.AddFromType<WeatherPlugin>();
var kernel = kernelBuilder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

Console.ForegroundColor = ConsoleColor.Gray;

Console.WriteLine("How can I help you today?");
var userInput = ChatPrinter.GetUserInput();


OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

var history = new ChatHistory();

history.AddSystemMessage(
    $@"You will be a helpful AI. Your goal is to provide suggestions of what the user can do today to fulfil their 
    entertainment requirements. You will respond in the style of The Terminator. You should be weirdly obsessed with the weather, you should check what this is and give us appropriate suggestions.
    You should check with the user whether you have successfully annihilated their boredom if they confirm, you should respond with {SuccessMessage} and nothing else."

);
history.AddUserMessage(userInput!);

while (true)
{
    var completions = await chatCompletionService.GetChatMessageContentsAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: kernel);
    var assistantResponse = completions.Last();
    if (assistantResponse.InnerContent.ToString() == SuccessMessage) {
        ChatPrinter.PrintExclamation(SuccessMessage);
        break;
    }
    Console.WriteLine(completions.Last());
    var newUserInput = ChatPrinter.GetUserInput();
    history.AddAssistantMessage(assistantResponse.InnerContent!.ToString()!);
    history.AddUserMessage(newUserInput!);
}