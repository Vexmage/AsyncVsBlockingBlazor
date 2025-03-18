using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using AsyncVsBlockingBlazor;
using AsyncVsBlockingBlazor.Data;
using AsyncVsBlockingBlazor.Services;  // Ensure Blazor can find AsyncTester

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Detect if running on GitHub Pages and adjust Base URI
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
#if RELEASE
    baseAddress = new Uri("https://Vexmage.github.io/AsyncVsBlockingBlazor/");  // Change this to match your GitHub Pages URL
#endif

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

// Register Database Context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=AsyncDemo.db"));

// Register AsyncTester as a Scoped Service (for DI)
builder.Services.AddScoped<AsyncTester>();

await builder.Build().RunAsync();
