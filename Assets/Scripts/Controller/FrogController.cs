using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorExtensions;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class FrogController : EnemyController
{
    [Header("属性")]
    [Range(0, 100)]
    public int jumpProbability; // 跳跃概率
    public float jumpForceX; // 跳跃水平方向力
    public float jumpForceY; // 跳跃竖直方向力

    [Header("检测辅助")]
    public LayerMask groundLayer;   // 地面

    private Collider2D coll;    // 碰撞体
    private Rigidbody2D rb;     // 刚体
    private Animator animator;  // 动画组件

    private float leftBorderX;  // 左边界
    private float rightBorderX; // 右边界

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // 获取边界位置
        leftBorderX = rightBorderX = transform.position.x;

        Transform leftBorder = transform.Find("LeftBorder");
        if (leftBorder)
        {
            leftBorderX = leftBorder.position.x;
            Destroy(leftBorder.gameObject);
        }

        Transform rightBorder = transform.Find("RightBorder");
        if (leftBorder)
        {
            rightBorderX = rightBorder.position.x;
            Destroy(rightBorder.gameObject);
        }

        // 初始化参数
        this.curHp = this.hp;

        // 初始化朝向
        UpdateFace(-1);
    }

    private void FixedUpdate() 
    {
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        if (IsOnGround())
        {
            animator.SetBool("bJump", false);
            animator.SetBool("bFall", false);
            return;
        }

        if (rb.velocity.y > 0)
        {
            animator.SetBool("bJump", true);
            animator.SetBool("bFall", false);
        }
        else if (rb.velocity.y < 0)
        {
            animator.SetBool("bJump", false);
            animator.SetBool("bFall", true);
        }
    }

    private int faceDir;

    private void Jump()
    {
        // 更新朝向
        if (transform.position.x <= leftBorderX)
        {
            UpdateFace(1);
        } 
        else if (transform.position.x >= rightBorderX)
        {
            UpdateFace(-1);
        }

        // 概率进行跳跃
        if (Random.Range(0, 100) < jumpProbability)
        {
            rb.velocity = new Vector2(jumpForceX * faceDir, jumpForceY);
        }
    }

    private void UpdateFace(int dir)
    {
        Vector3 scale = transform.localScale;

        faceDir = dir;
        scale.x = -faceDir * Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    public bool IsOnGround()
    {
        bool onGround = false;

        Vector3 collSize = coll.bounds.size;

        // 基础位置
        Vector2 baseFootPos = transform.position.XY() + coll.offset - new Vector2(0, collSize.y / 2);
        // 左脚位置
        Vector2 leftFootPos = baseFootPos - new Vector2(collSize.x / 2, 0);
        // 右脚位置
        Vector2 rightFootPos = baseFootPos + new Vector2(collSize.x / 2, 0);

        // 左脚检测
        onGround |= RaycastUtil.Raycast(leftFootPos, Vector2.down, groundLayer);
        // 右脚检测
        onGround |= RaycastUtil.Raycast(rightFootPos, Vector2.down, groundLayer);
        
        return onGround;
    }
}
