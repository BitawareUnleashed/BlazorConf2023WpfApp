using EventAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Services;

public class EventAggregatorService : ISubscriber<ValueChanged>
{
    private readonly IEventAggregator eventAggregator;

    public event EventHandler<string>? OnTimeSecondsChanged;

    void ISubscriber<ValueChanged>.OnEventRaised(ValueChanged e)
    {
        OnTimeSecondsChanged?.Invoke(this, e.Value);
    }

    public EventAggregatorService(IEventAggregator ea)
    {
        eventAggregator = ea;
        eventAggregator.Subscribe(this);
    }
}