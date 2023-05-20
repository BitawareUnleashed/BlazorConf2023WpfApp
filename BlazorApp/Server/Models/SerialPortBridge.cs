using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace BlazorApp.Server.Models;

public class SerialPortBridge
{
    private readonly ICommunicationServer hub;

    public Task<string> SendNotificationAsync(string data)
    {
        hub.Send(data);
        return Task.FromResult(data);
    }
}
