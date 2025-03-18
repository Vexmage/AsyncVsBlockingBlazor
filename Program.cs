using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using AsyncVsBlockingBlazor;
using AsyncVsBlockingBlazor.Data;
using AsyncVsBlockingBlazor.Services;  // Ensure Blazor can find AsyncTester

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Register Database Context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=AsyncDemo.db"));

// Register AsyncTester as a Scoped Service (for DI)
builder.Services.AddScoped<AsyncTester>();

await builder.Build().RunAsync();
