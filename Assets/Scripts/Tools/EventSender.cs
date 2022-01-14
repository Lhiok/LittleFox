using System;
using System.Collections.Generic;

public class EventSender<TKey, TValue> {
    // 事件表
    private Dictionary<TKey, Action<TValue>> dict = new Dictionary<TKey, Action<TValue>>();

    // 添加事件监听器
    public void AddListener(TKey eventType, Action<TValue> eventHandler)
    {
        if (dict.TryGetValue(eventType, out Action<TValue> callbacks))
        {
            dict[eventType] = callbacks + eventHandler;
        }
        else
        {
            dict.Add(eventType, eventHandler);
        }
    }

    // 移除事件监听器
    public void RemoveListener(TKey eventType, Action<TValue> eventHandler)
    {
        if (dict.TryGetValue(eventType, out Action<TValue> callbacks))
        {
            callbacks = (Action<TValue>) Delegate.RemoveAll(callbacks, eventHandler);
            if (callbacks == null)
            {
                dict.Remove(eventType);
            }
            else
            {
                dict[eventType] = callbacks;
            }
        }
    }

    // 判断某类型监听器是否存在
    public bool HasListener(TKey eventType)
    {
        return dict.ContainsKey(eventType);
    }

    // 派发事件
    public void Dispatch(TKey eventType, TValue eventArg)
    {
        if (dict.TryGetValue(eventType, out Action<TValue> callbacks))
        {
            callbacks.Invoke(eventArg);
        }
    }

    // 清除所有事件
    public void Clear()
    {
        dict.Clear();
    }
}
