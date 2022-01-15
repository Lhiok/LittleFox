using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    private Dictionary<ItemEnum, int> dict = new Dictionary<ItemEnum, int>();

    private Text text;
    private int totalVal;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInParent<Text>();

        if (text == null)
        {
            Debug.LogError("获取文本组件失败");
        }

        // 初始化分数
        totalVal = 0;
        text.text = "0";

        // 初始化得分规则
        dict.Add(ItemEnum.Cherry, 1);
        dict.Add(ItemEnum.Frog, 2);

        // 绑定事件
        EventUtil.AddListener(EventEnum.Item_Get, Score);
        EventUtil.AddListener(EventEnum.Kill_Enemy, Score);
    }

    private void Score(EventArgs eventArgs)
    {
        ItemEnum item = (ItemEnum) eventArgs.args[0];

        if (dict.TryGetValue(item, out int val))
        {
            totalVal += val;
            text.text = totalVal.ToString();
        }
    }
}
