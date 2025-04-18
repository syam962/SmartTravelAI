using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Travel.Web.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Travel.Web.Services;

namespace Travel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Kernel _kernel;
        private readonly ILocationService _locationService;

        public HomeController(ILogger<HomeController> logger, [FromKeyedServices("BookingsKernal")] Kernel kernel, ILocationService locationService)
        {
            _logger = logger;
            _kernel = kernel;
            _locationService = locationService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            /*
          
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
            chatHistory.AddMessage(result.Role, result?.Content!);*/
            var cities = await _locationService.GetAllCitiesAsync();

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
        public async Task<IActionResult> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = false, message = "Query cannot be empty." });
            }

            try
            {
                // Initialize chat history
                var chatHistory = new ChatHistory();

                // Get chat completion service
                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

                // Add the user's query to the chat history
                chatHistory.AddUserMessage(query);

                // Enable auto function calling
                var executionSettings = new OpenAIPromptExecutionSettings
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                };

                // Get the result from the AI
                var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel);

                // Add the AI's response to the chat history
                chatHistory.AddMessage(result.Role, result?.Content!);

                // Return the result as JSON
                return Json(new { success = true, data = result.Content });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during search.");
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }
    }
}
