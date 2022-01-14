// 事件参数
public class EventArgs
{
    public readonly string type;    // 事件类型
    public readonly object[] args;  // 事件参数

    public EventArgs(string type, params object[] args)
    {
        this.type = type;
        this.args = args;
    }
}