using System;
using System.Collections.Generic;

public class EventSender {
    // 事件表
    private Dictionary<string, EventListener> dict = new Dictionary<string, EventListener>();

    // 添加事件监听器
    public void AddListener(string eventType, EventHandler<EventArgs> eventHandler)
    {
        EventListener invoker;
        if (!dict.TryGetValue(eventType, out invoker))
        {
            invoker = new EventListener();
            dict.Add(eventType, invoker);
        }
        invoker.handler += eventHandler;
    }

    // 移除事件监听器
    public void RemoveListener(string eventType, EventHandler<EventArgs> eventHandler)
    {
        if (dict.TryGetValue(eventType, out EventListener invoker))
        {
            invoker.handler -= eventHandler;
        }
    }

    // 判断某类型监听器是否存在
    public bool HasListener(string eventType)
    {
        return dict.ContainsKey(eventType);
    }

    // 派发事件
    public void Dispatch(string eventType, params object[] eventArgs)
    {
        if (dict.TryGetValue(eventType, out EventListener invoker))
        {
            EventArgs args = new EventArgs(eventType, eventArgs);
            invoker.Invoke(args);
        }
    }

    // 清除所有事件
    public void Clear()
    {
        foreach (EventListener invoker in dict.Values)
        {
            invoker.Clear();
        }
        dict.Clear();
    }
}
