using Azure;
using Azure.AI.Inference;

var credentials = new AzureKeyCredential(System.Environment.GetEnvironmentVariable("GITHUB_TOKEN")!);

var client = new ChatCompletionsClient(new Uri("https://models.github.ai/inference"),
credentials);


var history = new ChatHistory()

while (true)
{
    Console.Write("Q:");
    string message = Console.ReadLine();

    var requestOptions = new ChatCompletionsOptions()
    {
        Messages =
    {
        new ChatRequestUserMessage(message),
    },
        Model = "meta/Llama-3.2-11B-Vision-Instruct",
        Temperature = 0.8f,
        MaxTokens = 2048,

    };

    Response<ChatCompletions> response = client.Complete(requestOptions);
    Console.WriteLine($"A:{response.Value.Content}");
}