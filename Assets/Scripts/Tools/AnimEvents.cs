using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    // 删除自身
    public void Damage()
    {
        Destroy(this.gameObject);
    }
}
