using System.Security.Principal;
using System.Text;
using Azure;
using Azure.Core.Pipeline;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using OpenAI;

var Github_Token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN");
Console.WriteLine($"token: {Github_Token}");
var model = "phi-3.5-mini-instruct";
var uri = "https://models.inference.ai.azure.com";

var client = new OpenAIClient(new AzureKeyCredential(Github_Token!), new OpenAIClientOptions
{
    Endpoint = new Uri(uri)
});

// create chat completion builder
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(model, client);

// get chat complettion service
var kernal = builder.Build();
var chat = kernal.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful assitant, If you don't know an answer then just say 'I don't know'");

var settings = new AzureOpenAIPromptExecutionSettings
{
    MaxTokens = 5000,
    Temperature=0.8
};

while (true)
{
    Console.Write("Q: ");

    var userMessage = Console.ReadLine();

    if (string.IsNullOrEmpty(userMessage)) break;
    history.AddUserMessage(userMessage);

    var response = new StringBuilder();
    //get response from AI

    Console.Write($"AI:");
    await foreach (var token in chat.GetStreamingChatMessageContentsAsync(history, settings, kernal))
    {
        response.Append(token.Content);
        Console.Write(token.Content);

    }

    Console.WriteLine();
    history.AddAssistantMessage(response.ToString());



}