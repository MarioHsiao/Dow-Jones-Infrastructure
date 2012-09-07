using System;

internal static class EventHandlerExtensions
{
    public static void Raise<T>(this EventHandler<T> handler, object sender = null, T args = null) where T : EventArgs
    {
        if (handler != null)
            handler(sender, args);
    }
}
