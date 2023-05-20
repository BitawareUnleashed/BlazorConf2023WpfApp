using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Server.Models;

public class CommunicationServer : ICommunicationServer
{
    private HubConnection? hubConnection;

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
    };

    public CommunicationServer() => _ = Init("https://localhost:7205/communicationhub");


    private async Task Init(string baseAddress)
    {
        hubConnection = new HubConnectionBuilder()
               .WithUrl(new Uri(baseAddress))
               .WithAutomaticReconnect(reconnectionTimeouts)
               .Build();
        await hubConnection.StartAsync();
    }

    /// <inheritdoc cref="ICommunicationServer" />
    public void Send(string toSend)
        => hubConnection?.SendAsync(nameof(ISpotHubClient.Message), toSend);

}
