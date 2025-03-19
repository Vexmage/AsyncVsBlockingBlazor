using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using AsyncVsBlockingBlazor;
using AsyncVsBlockingBlazor.Data;
using AsyncVsBlockingBlazor.Services; 

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Automatically adjust Base URI for GitHub Pages
var baseAddress = new Uri("https://vexmage.github.io/AsyncVsBlockingBlazor/");


// Debugging Tip: Log the base address
Console.WriteLine($"Base Address Set To: {baseAddress}");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

// Register Database Context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=AsyncDemo.db"));

// Register AsyncTester as a Scoped Service (for DI)
builder.Services.AddScoped<AsyncTester>();

await builder.Build().RunAsync();
