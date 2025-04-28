using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI; // Add this using directive to resolve 'Plugins'
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Travel.Web.Reposistory;
using Travel.Web.Services;
using Travel.Web.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddMemoryCache(); // Add this line to register IMemoryCache service
builder.Services.AddSingleton<ICacheService, CacheService>(); // Register ICacheService for dependency injection
// Add the following using directive at the top of the file to include the Npgsql.EntityFrameworkCore.PostgreSQL namespace


// Ensure that the Npgsql.EntityFrameworkCore.PostgreSQL NuGet package is installed in your project.
// You can install it using the following command in the Package Manager Console:
// Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
// Add PostgreSQL DbContext
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Register IUnitOfWork for dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Semantic Kernel to dependency injection
builder.Services.AddScoped<BookingsPlugin>();

// Register IFlightService for dependency injection
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IFlightBookingService, FlightBookingService>();


builder.Services.AddKeyedTransient<Kernel>("BookingsKernal", (sp, key) =>
{
    // Create a collection of plugins that the kernel will use
    KernelPluginCollection pluginCollection = new();
    pluginCollection.AddFromObject(sp.GetRequiredService<BookingsPlugin>());

    return new Kernel(sp, pluginCollection);
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
       // Add this using directive at the top of the file

        // Existing code remains unchanged
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "YourIssuer",
            ValidAudience = "YourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("YourSecretKey")) // This line now works
        };
    });

// Register AutoMapper for dependency injection
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


app.UseAuthorization();
var jwtSecret = builder.Configuration["Jwt:Secret"];
app.UseMiddleware<JwtCookieMiddleware>(jwtSecret);
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.UseRouting();

app.Run();


