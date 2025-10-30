using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

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
            var chatCompletionService = kernal.GetRequiredService<IChatCompletionService>();

            while (true)
            {
                Console.WriteLine("\nEnter your prompt: ");
                var prompt=Console.ReadLine();

                if (string.IsNullOrEmpty(prompt))
                    break;
                var response = await chatCompletionService.GetChatMessageContentAsync(prompt);
                Console.WriteLine(response);

            }
        }
    }
}
