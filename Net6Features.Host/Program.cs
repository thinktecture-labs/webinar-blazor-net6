using Blazored.LocalStorage;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using MudBlazor.Services;
using Net6Features.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var apiUrl = builder.Configuration.GetSection("api").GetValue<string>("baseUrl");
builder.Services.AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GrpcChannel>(services =>
{
    var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    var channel = GrpcChannel.ForAddress(apiUrl, new GrpcChannelOptions { HttpHandler = grpcWebHandler });

    return channel;
});

builder.Services.AddScoped<WeatherService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Call UseDeveloperExceptionPage on the app builder in the Development environment. 
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToPage("/_Host");
});
app.Run();
