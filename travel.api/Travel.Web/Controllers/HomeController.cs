using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Travel.Web.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Travel.Web.Services;

using Travel.Web.Infrastructure;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.AI;

namespace Travel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Kernel _kernel;
        private readonly ILocationService _locationService;
        private ICacheService _cacheService;
        public HomeController(ILogger<HomeController> logger, [FromKeyedServices("BookingsKernal")] Kernel kernel, ILocationService locationService, ICacheService cacheService)
        {
            _logger = logger;
            _kernel = kernel;
            _locationService = locationService;
            _cacheService = cacheService;
        }

        /*  public async Task<IActionResult> IndexAsync()
          {


              ChatHistory chatHistory = [];

              // Get chat completion service
              var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

              // Start the conversation
              string?promt_template= "<system_message> You are a flight booking assistant. Before making call to actual function find the required database ids to represent those eg, cityId, stateID, countrtyId etc, use function to find it.<system_message>{0}";
              string? input = "Give me list of flights availabe between 25 April 2025 and 26 April 2025 between Trivandrum to Bengaluru";
              string promt = string.Format(promt_template, input);





              // Add the message from the user to the chat history
              chatHistory.AddUserMessage(promt);

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
              var cities = await _locationService.GetAllCitiesAsync();

              return View();
          }
        */
        public async Task<IActionResult> IndexAsync()
        {
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

        public async Task<IActionResult> SearchAsync([FromBody] string query)
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
        public async Task<IActionResult> SearchAIAsync([FromBody] ReqObject query)
        {
            if (string.IsNullOrWhiteSpace(query.Query))
            {
                return Json(new { success = false, message = "Input cannot be empty." });
            }

            try
            {
                // Initialize chat history
                var chatHistory = new ChatHistory();

                List<ChatMessageContent> chat_his = _cacheService.Get<ChatMessageContent>("chat_history");

                if (chat_his != null)
                {
                    foreach (var item in chat_his)
                        chatHistory.AddMessage(item.Role, item?.Content!);
                }

                // Get chat completion service
                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

                // Add the user's input to the chat history
                chatHistory.AddUserMessage(query.Query);

                // Enable auto function calling
                var executionSettings = new OpenAIPromptExecutionSettings
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                };

                // Get the result from the AI
                var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel);



                _cacheService.AddToExistingCache("chat_history", result);

                // Return the result as JSON
                return Json(new { success = true, data = result.Content });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during AI search.");
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }

    }

    public class ReqObject
    {
        public string Query { get; set; }
    }


}
