using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxController : MonoBehaviour
{
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // 绑定事件
        EventUtil.AddListener(EventEnum.Fx_Play_Once, FxPlayOnce);
    }

    private void FxPlayOnce(EventArgs eventArgs)
    {
        GameObject prefab = (GameObject) eventArgs.args[0];
        Transform trans = (Transform) eventArgs.args[1];

        if (prefab == null)
        {
            return;
        }

        // 创建特效
        GameObject fx = Instantiate(prefab, trans.position, Quaternion.identity);
        fx.transform.parent = this.transform;
    }
}
