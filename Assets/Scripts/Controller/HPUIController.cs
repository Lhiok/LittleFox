using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    private Text text;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // 绑定事件
        EventUtil.AddListener(EventEnum.Update_Hp, UpdateUI);
    }

    private void UpdateUI(EventArgs eventArgs)
    {
        if (text == null)
        {
            text = GetComponentInParent<Text>();
        }

        int hp = (int) eventArgs.args[0];
        
        string val = "";
        for (int i = 0; i < hp; ++i)
        {
            val += "❤";
        }

        text.text = val;
    }
}
