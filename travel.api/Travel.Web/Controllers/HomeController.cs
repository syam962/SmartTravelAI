using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Travel.Web.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Travel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Kernel _kernel;

        public HomeController(ILogger<HomeController> logger, [FromKeyedServices("BookingsKernal")] Kernel kernel)
        {
            _logger = logger;
            _kernel = kernel;
        }

        public async Task<IActionResult> IndexAsync()
        {

          
            ChatHistory chatHistory = [];

            // Get chat completion service
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            // Start the conversation
            string? input = "Books a new table at a restaurant";





            // Add the message from the user to the chat history
            chatHistory.AddUserMessage(input);

            // Enable auto function calling
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            // Get the result from the AI
            var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel);

            // Print the result
            Console.WriteLine("Assistant > " + result);

            // Add the message from the agent to the chat history
            chatHistory.AddMessage(result.Role, result?.Content!);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
