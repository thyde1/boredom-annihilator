using Azure.Core.Cryptography;
using Hackathon_Neworbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using TMDbLib.Client;

const string SuccessMessage = "***BOREDOM ANNIHILATED***";

var builder = new HostApplicationBuilder();
builder.Configuration.AddUserSecrets("c0515972-21aa-4f00-af01-2e17a542c4e1");
var application = builder.Build();
var configuration = application.Services.GetRequiredService<IConfiguration>();

Console.WriteLine("BOREDOM ANNIHILATOR (v1)\n");

var kernelBuilder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion("gpt4o", "https://boredom-euw-ai.openai.azure.com/", configuration.GetValue<string>("openaiKey")!);
kernelBuilder.Plugins.Services.AddHttpClient();
kernelBuilder.Plugins.Services.AddSingleton<IConfiguration>(configuration);
kernelBuilder.Plugins.AddFromType<WeatherPlugin>();
kernelBuilder.Plugins.Services.AddTransient<TMDbClient>(services =>
{
    var config = services.GetRequiredService<IConfiguration>();
    var key = configuration.GetValue<string>("tmdbKey")
        ?? throw new ArgumentException("weatherKey not provided");
    return new TMDbClient(key);
});
kernelBuilder.Plugins.AddFromType<TmdbPlugin>();
kernelBuilder.Plugins.AddFromType<GeoApifyPlugin>();
var kernel = kernelBuilder.Build();

OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

Console.ForegroundColor = ConsoleColor.Gray;

Console.WriteLine("How can I help you today?");
var userInput = ChatPrinter.GetUserInput();

var history = new ChatHistory();

history.AddSystemMessage(
    $@"You will be a helpful AI. Your goal is to provide suggestions of what the user can do today to fulfil their 
    entertainment requirements. You will respond in the style of The Terminator. You should be weirdly obsessed with the weather, you should check what this is and give us appropriate suggestions.
    You should check with the user whether you have successfully annihilated their boredom if they confirm, you should respond with {SuccessMessage} and nothing else."
);
history.AddUserMessage(userInput!);

while (true)
{
    var response = await GetChatCompletion(chatCompletionService, history);
    if (response == SuccessMessage) {
        ChatPrinter.PrintExclamation(SuccessMessage);
        break;
    }
    Console.WriteLine(response);
    var newUserInput = ChatPrinter.GetUserInput();
    history.AddAssistantMessage(response);
    history.AddUserMessage(newUserInput!);
}

async Task<string> GetChatCompletion(IChatCompletionService chatCompletionService, ChatHistory history)
{
    IReadOnlyList<ChatMessageContent> completions = null;
    while (completions == null)
    {
        try
        {
            completions = await chatCompletionService.GetChatMessageContentsAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: kernel);
        }
        catch (HttpOperationException e)
        {
            var delaySeconds = int.Parse(e.Message.Split("after ")[1].Split(" seconds")[0]);
            ChatPrinter.PrintInfo($"Throttled. Waiting ({delaySeconds} seconds)...");
            await Task.Delay(delaySeconds * 1000);
        }
    }

    var assistantResponse = completions.Last();
    return assistantResponse.InnerContent.ToString();
}
