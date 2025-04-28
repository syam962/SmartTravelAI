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
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Travel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Kernel _kernel;
        private readonly ILocationService _locationService;
        private ICacheService _cacheService;
        private readonly IFlightService _flightService;
        public HomeController(ILogger<HomeController> logger, [FromKeyedServices("BookingsKernal")] Kernel kernel, ILocationService locationService, ICacheService cacheService, IFlightService flightService)
        {
            _logger = logger;
            _kernel = kernel;
            _locationService = locationService;
            _cacheService = cacheService;
            _flightService = flightService;
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
                int userID = 0;
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userIdStr)
                {
                    userID = Convert.ToInt32(userIdStr);
                }
                // Initialize chat history  
                ChatHistory chatHistory;
                Dictionary<string, object> functionArgs = new Dictionary<string, object>();
                var history = _cacheService.Get<ChatHistory>("chat_history");
                if (history == null)
                {
                    chatHistory = new ChatHistory();
                }
                else
                {
                    chatHistory = history;
                }
                string lastfunctionCalled = string.Empty;
                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

                // Add the user's input to the chat history  
                string messageTemplate = string.Format("Current userId :{0}.{1}", userID,query.Query);
                chatHistory.AddUserMessage(messageTemplate);
                string chatResult = string.Empty;

                // Enable auto function calling  
                var executionSettings = new OpenAIPromptExecutionSettings
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: false)
                };

                while (true)
                {
                    // Get the result from the AI  
                    var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel);

                    if (result.Content is not null)
                    {
                        chatResult = result.Content;
                        break;
                    }

                    chatHistory.Add(result);
                    IEnumerable<Microsoft.SemanticKernel.FunctionCallContent> functionCalls = Microsoft.SemanticKernel.FunctionCallContent.GetFunctionCalls(result);
                    foreach (Microsoft.SemanticKernel.FunctionCallContent functionCall in functionCalls)
                    {
                        try
                        {
                            lastfunctionCalled = functionCall.FunctionName.ToLower();
                            var args = functionCall.Arguments;
                            foreach (var arg in args)
                            {
                                functionArgs.Add(arg.Key, arg.Value);
                            }
                            // Invoking the function  
                            Microsoft.SemanticKernel.FunctionResultContent resultContent = await functionCall.InvokeAsync(_kernel);

                            // Adding the function result to the chat history  
                            chatHistory.Add(resultContent.ToChatMessage());
                        }
                        catch (Exception ex)
                        {
                            // Adding function exception to the chat history  
                            chatHistory.Add(new Microsoft.SemanticKernel.FunctionResultContent(functionCall, ex).ToChatMessage());
                        }
                    }
                }


                _cacheService.AddToExistingCache("chat_history", chatHistory);

                ResponseObject objResult = new ResponseObject();
                objResult.ChatSummary = chatResult;
                objResult.FunctionName = lastfunctionCalled;
                objResult.FunctionArgs = functionArgs;
                return Json(new { success = true, data = objResult });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during AI search.");
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }

        public async Task<IActionResult> LoadFlightBooking(int segmentID)
        {
            // Get the flight details using the flightID
            FlightSegment flightDetails = await _flightService.GetFlightSegmentDetailsAsync(segmentID);
            return PartialView("flight_booking", flightDetails);
        }

        public async  Task<IActionResult> Login()
        {
            return View();

        }


    }

    public class ReqObject
    {
        public string Query { get; set; }
    }
    public class ResponseObject
    {
        public string ChatSummary { get; set; }

        public string FunctionName { get; set; }
        public string segmentId { get; set; }

        public Dictionary<string, object> FunctionArgs { get; set; }
    }

}
