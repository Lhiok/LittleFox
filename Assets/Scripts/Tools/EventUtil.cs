using System;

// 事件工具
public static class EventUtil
{
    // 事件发送器
    private static EventSender sender = new EventSender();

    // 添加事件监听器
    public static void AddListener(string eventType, EventHandler<EventArgs> eventHandler)
    {
        sender.AddListener(eventType, eventHandler);
    }

    // 移除事件监听器
    public static void RemoveListener(string eventType, EventHandler<EventArgs> eventHandler)
    {
        sender.RemoveListener(eventType, eventHandler);
    }

    // 判断某类型监听器是否存在
    public static bool HasListener(string eventType)
    {
        return sender.HasListener(eventType);
    }

    // 派发事件
    public static void Dispatch(string eventType, params object[] eventArgs)
    {
        sender.Dispatch(eventType, eventArgs);
    }

    // 清除所有事件
    public static void Clear()
    {
        sender.Clear();
    }
}