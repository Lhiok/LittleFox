using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb; // 父组件的刚体组件

    public float speed; // 移速

    // Start is called before the first frame update
    void Start()
    {
        // 获取父组件的刚体组件
        rb = GetComponentInParent<Rigidbody2D>();
        
        if (!rb)
        {
            Debug.Log("PlayerController: Failed to get rigidbody2D in parent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // 移动
    private void Move()
    {
        // 无法移动不处理
        if (!moveable()) 
        {
            return;
        }

        // 获取输入
        float horizontalSpeed = speed * Input.GetAxis("Horizontal");
        float verticalSpeed = rb.velocity.y;

        // 设置速度
        // rb.velocity.Set(horizontalSpeed, verticalSpeed);
        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }

    // 能否移动
    private bool moveable() {
        if (!rb)
        {
            return false;
        }

        return true;
    }
}
