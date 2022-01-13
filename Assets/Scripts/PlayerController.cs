using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorExtensions;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float mvoeSpeed; // 移速
    public float jumpForce; // 跳跃力
    public int maxJumpCount;    // 多段跳次数

    [Header("检测辅助")]
    public LayerMask groundLayer;   // 地面
    public float rayLineLength; // 检测线长度

    private Rigidbody2D rb;     // 刚体组件
    private Animator animator;  // 动画组件
    private Collider2D coll;    // 碰撞组件

    // Start is called before the first frame update
    void Start()
    {
        // 获取刚体组件
        rb = GetComponentInParent<Rigidbody2D>();
        // 获取动画组件
        animator = GetComponentInParent<Animator>();
        // 获取碰撞组件
        coll = GetComponentInParent<Collider2D>();

        if (!rb)
        {
            Debug.LogError("获取刚体组件失败");
        }

        if (!animator)
        {
            Debug.LogError("获取动画组件失败");
        }
        
        if (!coll)
        {
            Debug.LogError("获取碰撞组件失败");
        }
    }

    private bool wJump = false; // 记录跳跃键状态在下一逻辑帧更新
    private int jumpCount = 0;  // 剩余跳跃次数

    // Update is called once per frame
    void Update()
    {
        // 检测跳跃键
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            wJump = true;
        }
    }

    void FixedUpdate()
    {
        // 重置跳跃
        ResetJump();
        // 操作更新
        Move();
        // 更新动画
        UpdateAnim();
    }

    // 重置跳跃
    private void ResetJump()
    {
        if (IsOnGround())
        {
            jumpCount = maxJumpCount;
        }
    }

    // 移动
    private void Move()
    {
        // 无法操作不处理
        if (!Moveable()) 
        {
            return;
        }

        // 获取输入
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        // 设置速度
        float horizontalSpeed = horizontalMove * mvoeSpeed * Time.fixedDeltaTime;
        float verticalSpeed = rb.velocity.y;

        // 根据跳跃键状态设置竖直速度
        if (wJump)
        {
            // 重置wJump
            wJump = false;
            --jumpCount;
            verticalSpeed = jumpForce * Time.fixedDeltaTime;
        }

        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        // 设置朝向
        if (horizontalMove != 0)
        {
            Vector3 scale = rb.transform.localScale;
            float scaleX = horizontalMove * Mathf.Abs(scale.x);

            rb.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        }
    }

    // 切换动画
    private void UpdateAnim()
    {
        // 移动
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        // 起跳
        if (rb.velocity.y > 0)
        {
            animator.SetBool("bJump", true);
        }
        // 下落
        else if (!IsOnGround() && rb.velocity.y < 0)
        {
            animator.SetBool("bFall", true);
            animator.SetBool("bJump", false);
        }
        // 落地/悬浮
        else
        {
            animator.SetBool("bJump", false);
            animator.SetBool("bFall", false);
        }
    }

    public bool IsOnGround()
    {
        bool onGround = false;

        // 碰撞组件大小
        Vector3 collisionSize = coll.bounds.size;
        // 基础位置
        Vector2 baseFootPos = transform.position.XY() + coll.offset - new Vector2(0, collisionSize.y / 2);
        // 左脚位置
        Vector2 leftFootPos = baseFootPos - new Vector2(collisionSize.x / 2, 0);
        // 右脚位置
        Vector2 rightFootPos = baseFootPos + new Vector2(collisionSize.x / 2, 0);

        // 左脚检测
        onGround |= Raycast(leftFootPos, Vector2.down, rayLineLength, groundLayer);
        // 右脚检测
        onGround |= Raycast(rightFootPos, Vector2.down, rayLineLength, groundLayer);
        
        return onGround;
    }

    // 能否移动
    public bool Moveable() 
    {
        return true;
    }

    // 重载Raycast方法
    public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
    {
        RaycastHit2D result = Physics2D.Raycast(origin, direction, distance, layerMask);
        Debug.DrawRay(origin, direction, result? Color.red: Color.green, distance);
        return result;
    }
}
