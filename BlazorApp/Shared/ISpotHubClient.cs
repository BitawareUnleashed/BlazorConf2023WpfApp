public interface ISpotHubClient
{
    Task Message(string message);
}