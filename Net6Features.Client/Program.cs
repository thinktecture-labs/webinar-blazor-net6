using Blazored.LocalStorage;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using MudBlazor.Services;
using Net6Features.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");
var apiUrl = builder.Configuration.GetSection("api").GetValue<string>("baseUrl");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

builder.Services.AddSingleton(services =>
{
    // Get the service address from appsettings.json
    var config = services.GetRequiredService<IConfiguration>();
    var backendUrl = config.GetSection("api").GetValue<string>("baseUrl");

    // Create a channel with a GrpcWebHandler that is addressed to the backend server.
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());

    return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
});

builder.Services.AddSingleton<WeatherService>();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
