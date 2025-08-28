using cmac.demo.Client.Pages;
using cmac.demo.Components;
using cmac.demo.data;
using cmac.demo.Extensions;

using Microsoft.EntityFrameworkCore;

using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DonationDb"));

// Required in to allow for HttpClient to be injected in the WebAssembly host
builder.Services.AddHttpClient("YourApp.ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7156");
});

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("YourApp.ServerAPI"));


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(cmac.demo.Client._Imports).Assembly);

app.SetupEndpoints();

// Add seed data to the InMemory database
app.SetupSeedData();

app.Run();