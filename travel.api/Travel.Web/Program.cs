using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI; // Add this using directive to resolve 'Plugins'

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bind configuration for OpenAI settings
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));

// Register IChatCompletionService with settings from appsettings.json
builder.Services.AddSingleton<IChatCompletionService>(sp =>
{
    var openAISettings = sp.GetRequiredService<IOptions<OpenAISettings>>().Value;
    return new OpenAIChatCompletionService(
        modelId: openAISettings.ModelId,
        apiKey: openAISettings.ApiKey
    );
});

// Add Semantic Kernel to dependency injection
builder.Services.AddSingleton<BookingsPlugin>();

builder.Services.AddKeyedTransient<Kernel>("BookingsKernal", (sp, key) =>
{
    // Create a collection of plugins that the kernel will use
    KernelPluginCollection pluginCollection = new();
    pluginCollection.AddFromObject(sp.GetRequiredService<BookingsPlugin>());

    return new Kernel(sp, pluginCollection);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

// Supporting class for OpenAI settings
public class OpenAISettings
{
    public string ModelId { get; set; }
    public string ApiKey { get; set; }
}

