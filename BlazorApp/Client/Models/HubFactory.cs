using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Client.Models;
/// <summary>
/// Factory able to build a SignalR client Hub
/// </summary>
public class HubFactory
{
    public NavigationManager NavManager { get; set; }

    public HttpClient Http { get; set; }

    private readonly string hubEndpoint = "https://localhost:7205/communicationhub";

    /// <summary>
    /// Reconnection timings policy for SignalR Hub able to automatic reconnect if the connection is lost.
    /// </summary>
    private readonly TimeSpan[] reconnectionTimeouts =
    {
        TimeSpan.FromSeconds(0),
        TimeSpan.FromSeconds(0),
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(15),
        TimeSpan.FromSeconds(15),
        TimeSpan.FromSeconds(30),
        TimeSpan.FromSeconds(60),
    };

    public HubFactory(HttpClient http, NavigationManager navManager)
    {
        Http = http;
        NavManager = navManager;
    }

    /// <summary>
    /// Create a new Connections of SignalR client.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <param name="handler">The handler.</param>
    /// <returns></returns>
    public async Task<HubConnection> Connection<T1>(Action<T1> handler)
    {
        var hubConnection = new HubConnectionBuilder()
                        .WithUrl(NavManager.ToAbsoluteUri(hubEndpoint), HttpTransportType.WebSockets,
                                 options =>
                                 {
                                     options.CloseTimeout = TimeSpan.FromSeconds(5);
                                     options.DefaultTransferFormat = TransferFormat.Binary;
                                     options.TransportMaxBufferSize = 15_000_000;
                                     options.SkipNegotiation = true;
                                 })
                        .WithAutomaticReconnect(reconnectionTimeouts)
                        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Information))
                        .Build();
        try
        {

            hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(10);

            hubConnection.On(nameof(ISpotHubClient.Message), handler);

            await hubConnection.StartAsync();

        }
        catch (Exception ex)
        {
            string s = ex.Message;
        }
        return hubConnection;
    }
}
