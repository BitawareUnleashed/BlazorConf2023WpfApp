namespace BlazorApp.Server.Models;

public interface ICommunicationServer
{
    void Send(string toSend);
}
