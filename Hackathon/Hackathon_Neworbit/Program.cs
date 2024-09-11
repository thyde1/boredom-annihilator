using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("BOREDOM ANNIHIALATOR (v1)\n");

Console.WriteLine("Boredom annihilation in progress...");

var kernel = Kernel.
    CreateBuilder().AddAzureOpenAIChatCompletion("gpt4o", "https://boredom-euw-ai.openai.azure.com/", "5e24fb3095484dbfa47db36f61f9feb4")
    .Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.Add(new ChatMessageContent(AuthorRole.User, "How can I annihilate my boredom?"));
var completions = await chatCompletionService.GetChatMessageContentsAsync(history);
Console.WriteLine(completions.Last());
