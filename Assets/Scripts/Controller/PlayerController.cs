using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorExtensions;

public class PlayerController : MonoBehaviour
{
    [Header("参数")]
    public int hp;  // 血量
    public float mvoeSpeed; // 移速
    public float jumpForce; // 跳跃力
    public int maxJumpCount;    // 多段跳次数

    [Header("检测辅助")]
    public LayerMask groundLayer;   // 地面
    public float rayLineLength; // 检测线长度

    private Rigidbody2D rb;     // 刚体组件
    private Animator animator;  // 动画组件
    private Collider2D coll;    // 碰撞组件

    private int curHp;  // 剩余血量
    private bool wJump; // 记录跳跃键状态在下一逻辑帧更新
    private int jumpCount;  // 剩余跳跃次数

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

        // 参数初始化
        curHp = hp;
        wJump = false;
        jumpCount = maxJumpCount;

        // UI初始化
        EventUtil.Dispatch(EventEnum.Update_Hp, curHp);

        // 绑定事件
        EventUtil.AddListener(EventEnum.Player_Bounce, Bounce);
        EventUtil.AddListener(EventEnum.Player_Hurt, Hurt);
    }
    
    // 被弹飞
    private void Bounce(EventArgs eventArgs)
    {
        Vector2 force = (Vector2) eventArgs.args[0];

        // 受伤状态无法弹飞
        if (animator.GetBool("Hurt") || force == null)
        {
            return;
        }

        rb.AddForce(force);
        animator.SetBool("bJump", true);
        animator.SetBool("bFall", false);
    }

    // 受伤
    private void Hurt(EventArgs eventArgs)
    {
        int damage = (int) eventArgs.args[0];
        Vector2 force = (Vector2) eventArgs.args[1];

        curHp = Mathf.Max(0, curHp - damage);
        EventUtil.Dispatch(EventEnum.Update_Hp, curHp);

        // 角色死亡
        if (curHp == 0)
        {
            animator.SetBool("Die", true);
            EventUtil.Dispatch(EventEnum.Player_Die);
            return;
        }

        animator.SetBool("Hurt", true);

        // 有受力方向
        if (force != null)
        {
            rb.AddForce(force);
        }
    }

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
        // 受伤切待机
        if (animator.GetBool("Hurt"))
        {
            if (Mathf.Abs(rb.velocity.x) < 0.01f && Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                animator.SetBool("Hurt", false);
                rb.velocity = new Vector2(0, 0);
            }
        }

        // 移动
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        // 起跳
        if (rb.velocity.y > 0)
        {
            animator.SetBool("bJump", true);
            animator.SetBool("bFall", false);
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
        // 左脚位置 取1/3防止边缘接触墙面
        Vector2 leftFootPos = baseFootPos - new Vector2(collisionSize.x / 3, 0);
        // 右脚位置 取1/3防止边缘接触墙面
        Vector2 rightFootPos = baseFootPos + new Vector2(collisionSize.x / 3, 0);

        // 左脚检测
        onGround |= Raycast(leftFootPos, Vector2.down, rayLineLength, groundLayer);
        // 右脚检测
        onGround |= Raycast(rightFootPos, Vector2.down, rayLineLength, groundLayer);
        
        return onGround;
    }

    // 能否移动
    public bool Moveable() 
    {
        if (animator.GetBool("Die") || animator.GetBool("Hurt"))
        {
            return false;
        }

        return true;
    }

    // 重载Raycast方法
    public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
    {
        RaycastHit2D result = Physics2D.Raycast(origin, direction, distance, layerMask);
        Debug.DrawRay(origin, direction * distance, result? Color.red: Color.green);
        return result;
    }
}
