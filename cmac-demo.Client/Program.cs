using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<ThemeService>();
builder.Services.AddRadzenComponents();

// Required in to allow for HttpClient to be injected in the WebAssembly client
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();