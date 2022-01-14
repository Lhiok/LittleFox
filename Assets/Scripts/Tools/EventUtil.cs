using System;

// 事件工具
public static class EventUtil
{
    // 事件发送器
    private static EventSender<EventEnum, object> sender = new EventSender<EventEnum, object>();

    // 添加事件监听器
    public static void AddListener(EventEnum eventType, Action<object> eventHandler)
    {
        sender.AddListener(eventType, eventHandler);
    }

    // 移除事件监听器
    public static void RemoveListener(EventEnum eventType, Action<object> eventHandler)
    {
        sender.RemoveListener(eventType, eventHandler);
    }

    // 判断某类型监听器是否存在
    public static bool HasListener(EventEnum eventType)
    {
        return sender.HasListener(eventType);
    }

    // 派发事件
    public static void Dispatch(EventEnum eventType)
    {
        sender.Dispatch(eventType, null);
    }

    // 派发事件
    public static void Dispatch(EventEnum eventType, object eventArg)
    {
        sender.Dispatch(eventType, eventArg);
    }

    // 清除所有事件
    public static void Clear()
    {
        sender.Clear();
    }
}