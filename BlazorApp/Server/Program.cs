using BlazorApp.Server.API;
using BlazorApp.Server.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<SerialCommunication>();
builder.Services.AddSingleton<ICommunicationServer, CommunicationServer>();

builder.Services.AddSignalR(
    hubOptions =>
    {
        hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(1);
        hubOptions.MaximumReceiveMessageSize = 65_536;
        hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
        hubOptions.MaximumParallelInvocationsPerClient = 16;
        hubOptions.EnableDetailedErrors = true;
        hubOptions.StreamBufferCapacity = 15;
    }).AddJsonProtocol(
    options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.Encoder = JavaScriptEncoder.Default;
        options.PayloadSerializerOptions.IncludeFields = false;
        options.PayloadSerializerOptions.IgnoreReadOnlyFields = false;
        options.PayloadSerializerOptions.IgnoreReadOnlyProperties = false;
        options.PayloadSerializerOptions.MaxDepth = 0;
        options.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
        options.PayloadSerializerOptions.DictionaryKeyPolicy = null;
        options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
        options.PayloadSerializerOptions.DefaultBufferSize = 32_768;
        options.PayloadSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        options.PayloadSerializerOptions.ReferenceHandler = null;
        options.PayloadSerializerOptions.UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement;
        options.PayloadSerializerOptions.WriteIndented = true;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseSerialSendApi();

app.MapRazorPages();
app.MapControllers();


//app.MapHub<CommunicationHub>("/communicationhub", options =>
//{
//    options.Transports = HttpTransportType.WebSockets;
//    options.CloseOnAuthenticationExpiration = false;
//    options.ApplicationMaxBufferSize = 65_536;
//    options.TransportMaxBufferSize = 65_536;
//    options.MinimumProtocolVersion = 0;
//    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
//    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(3);
//});

app.MapHub<CommunicationHub>("/communicationhub", options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
    options.CloseOnAuthenticationExpiration = false;
    options.ApplicationMaxBufferSize = 65_536;
    options.TransportMaxBufferSize = 65_536;
    options.MinimumProtocolVersion = 0;
    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(3);
});

app.Run();


