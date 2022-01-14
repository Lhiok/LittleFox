using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionController : MonoBehaviour
{
    [Header("物品名称")]
    public ItemEnum itemName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 玩家触碰到可收集物
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            // 派发事件
            EventUtil.Dispatch(EventEnum.Item_Get, itemName);
        }
    }
}
