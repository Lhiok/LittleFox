using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectionController : MonoBehaviour
{
    [Header("物品属性")]
    public ItemEnum itemName;

    [Header("特效")]
    public GameObject fxItemGet; 

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // 玩家触碰到可收集物
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            // 派发事件
            EventUtil.Dispatch(EventEnum.Item_Get, itemName);
            EventUtil.Dispatch(EventEnum.Fx_Play_Once, fxItemGet, transform);
        }
    }
}
