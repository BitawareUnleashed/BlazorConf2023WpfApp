namespace EventAggregator.Models;
public interface ISubscriber<TEventType>
{
    void OnEventRaised(TEventType e);
}