using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;

namespace SemanticKernalDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Read Readme file how yo can create azure account and can get apiKey and other string values to run the prompt

            var modelId = "";
            var apiKey = "";
            var endpoint = "";

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(modelId,endpoint,apiKey);
            Kernel kernal=builder.Build();

            //Create Chat History
            var history = new ChatHistory();

            var chatCompletionService = kernal.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings settings = new()
            {
                ChatSystemPrompt = "You are a friendly AI Assistant that answer in friendly manners",
                Temperature=1,
                //MaxTokens=100,
            };

            while (true)
            {
                Console.WriteLine("\nEnter your prompt: ");
                var prompt=Console.ReadLine();

                if (string.IsNullOrEmpty(prompt))
                    break;
                history.AddUserMessage(prompt);
                var response = await chatCompletionService.GetChatMessageContentAsync(history,settings);
                
                //Add response to chat History
                history.Add(response);
                
                ChatTokenUsage usage = ((OpenAI.Chat.ChatCompletion)response.InnerContent!).Usage;
                Console.WriteLine(response);
                Console.WriteLine($"\n \tInput Token: \t{usage.InputTokenCount}");
                Console.WriteLine($"\n \tInput Token: \t{usage.OutputTokenCount}");
                Console.WriteLine($"\n \tInput Token: \t{usage.TotalTokenCount}");

            }
        }
    }
}
