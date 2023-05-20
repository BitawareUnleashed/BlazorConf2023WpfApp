using BlazorApp.Server.Models;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace BlazorApp.Server.API;

public static class SeriapPortAPI
{
    public static IEndpointRouteBuilder UseSerialSendApi(this IEndpointRouteBuilder app)
    {
        _ = app.MapGroup("blazor/conf/2023/")
            .MapApi();
        return app;
    }

    private static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
    {
        _ = group.MapPost($"v1/SendStringToSerialPort", SendSerialApi);
        _ = group.MapPost($"v1/SetSerialPortConnect", SetSerialConnectionApi);
        _ = group.MapGet("v1/SendSerialPortCloseConnection", SerialDisconnectApi);
        return group;
    }

    private static async Task<IResult> SendSerialApi(HttpContext context, SerialCommunication serial)
    {
        using (var reader = new StreamReader(context.Request.Body))
        {
            var requestData = await reader.ReadToEndAsync();
            serial.SerialCmdSend(requestData);
        }
        return Results.Ok();
    }

    private static IResult SerialDisconnectApi(HttpContext context, SerialCommunication serial)
    {
        serial.Disconnect();
        return Results.Ok();
    }

    private static async Task<IResult> SetSerialConnectionApi(HttpContext context, SerialCommunication serial)
    {
        using (var reader = new StreamReader(context.Request.Body))
        {
            var requestData = await reader.ReadToEndAsync();

            var connectionData=JsonSerializer.Deserialize<SerialData>(requestData);

            serial.ConnectToSerial(connectionData.PortName, connectionData.BaudRate);

        }
        return Results.Ok();
    }
}
