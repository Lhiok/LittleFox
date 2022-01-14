public delegate void EventHandler<T>(T args);

public class EventListener
{
    public EventHandler<EventArgs> handler;

    public void Invoke(EventArgs args)
    {
        if (handler != null)
        {
            handler.Invoke(args);
        }
    }

    public void Clear()
    {
        handler = null;
    }
}