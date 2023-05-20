using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Server.Models;

public class CommunicationHub : Hub<ISpotHubClient>
{
    #region Methods and callbacks

    /// <inheritdoc cref="ISpotHubClient">
    public async Task Message(string message) => await Clients.All.Message(message);

    #endregion
}
