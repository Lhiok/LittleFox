using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EagleController : EnemyController
{
    [Header("属性")]
    public float speed; // 速度

    private Rigidbody2D rb;     // 刚体
    private float topBorderY;   // 上边界
    private float bottomBorderY;    // 下边界

    private int moveDir;    // 移动方向 -1：下降 1：上升

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();

        // 初始化参数
        curHp = hp;
        moveDir = 1;

        // 获取边界位置
        topBorderY = bottomBorderY = transform.position.y;

        Transform topBorder = transform.Find("TopBorder");
        if (topBorder)
        {
            topBorderY = topBorder.position.y;
            Destroy(topBorder.gameObject);
        }

        Transform bottomBorder = transform.Find("BottomBorder");
        if (bottomBorder)
        {
            bottomBorderY = bottomBorder.position.y;
            Destroy(bottomBorder.gameObject);
        }
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private void Move()
    {
        // 更新方向
        if (transform.position.y > topBorderY)
        {
            moveDir = -1;
        }
        else if (transform.position.y < bottomBorderY)
        {
            moveDir = 1;
        }

        // 移动
        Vector2 v = rb.velocity;
        v.y = speed * Time.fixedDeltaTime * moveDir;
        rb.velocity = v;
    }
}
