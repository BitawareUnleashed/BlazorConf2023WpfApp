using BlazorApp.Client.Models;
using BlazorApp.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace BlazorApp.Client.Services;

public class SerialPortService
{
    private readonly HubFactory hubFactory;

    public event EventHandler<string>? NotificationReceived;
    public event EventHandler<bool>? SerialPortStateChanged;

    /// <summary>
    /// Gets or sets the HTTP client.
    /// </summary>
    /// <value>
    /// The HTTP.
    /// </value>
    public HttpClient Http { get; set; }

    public SerialPortService(HttpClient http, HubFactory hubFactory)
    {
        Http = http;
        this.hubFactory = hubFactory;

        _ = InitializeNotifications();
        
    }

    /// <summary>
    /// Sends the data to serial port through HTTP.
    /// </summary>
    /// <param name="data">The data.</param>
    public async Task SendDataToSerialPort(string data)
    {
        var requestContent = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        var requestAddr = $"blazor/conf/2023/v1/SendStringToSerialPort";
        await Http.PostAsync(requestAddr, requestContent).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends to the serial port the baud rate.
    /// </summary>
    public async Task SendSerialPortBaudRate(string data)
    {
        var requestContent = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        var requestAddr = $"blazor/conf/2023/v1/SeeSerialPortBaudRate";
        await Http.PostAsync(requestAddr, requestContent).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends the name to the serial port.
    /// </summary>
    public async Task SendSerialPortName(string data)
    {
        var requestContent = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        var requestAddr = $"blazor/conf/2023/v1/SetSerialPortName";
        await Http.PostAsync(requestAddr, requestContent).ConfigureAwait(false);
    }

    public async Task Disconnect()
    {
        var requestAddr = $"blazor/conf/2023/v1/SendSerialPortCloseConnection";
        await Http.GetAsync(requestAddr).ConfigureAwait(false);
    }

    public async Task Connect(string comName, int baudRate)
    {
        var data = new SerialData(comName, baudRate);
        
        var jsonData = JsonSerializer.Serialize(data);
        var requestContent = new StringContent(jsonData, Encoding.UTF8, MediaTypeNames.Application.Json);
        var requestAddr = $"blazor/conf/2023/v1/SetSerialPortConnect";
        await Http.PostAsync(requestAddr, requestContent).ConfigureAwait(false);
    }








    /// <summary>
    /// Gets or sets the hub connection.
    /// </summary>
    /// <value>
    /// The hub connection.
    /// </value>
    public HubConnection? HubConnection { get; set; }

    /// <summary>
    /// Initializes the notifications.
    /// </summary>
    /// <exception cref="InvalidDataException"></exception>
    private async Task InitializeNotifications() => HubConnection = await hubFactory.Connection<string>
            (HubConnection_Notifications);

    /// <summary>
    /// Notifications from hubs connection.
    /// </summary>
    /// <param name="context">The context.</param>
    private void HubConnection_Notifications(string context)
    {
        NotificationReceived?.Invoke(this, context);
        if(context.Contains("Port open on"))
        {
            SerialPortStateChanged?.Invoke(this, true);
        }

        if (context.Contains("Disconnected"))
        {
            SerialPortStateChanged?.Invoke(this, false);
        }
    }
}
