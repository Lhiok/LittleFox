using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb; // 父组件的刚体组件
    private Animator animator;  // 父组件的动画组件
    private Collider2D coll;    // 父组件的碰撞组件

    public float mvoeSpeed; // 移速
    public float jumpForce; // 跳跃力

    // Start is called before the first frame update
    void Start()
    {
        // 获取父组件的刚体组件
        rb = GetComponentInParent<Rigidbody2D>();
        
        if (!rb)
        {
            Debug.Log("PlayerController: Failed to get rigidbody2D in parent");
        }

        animator = GetComponentInParent<Animator>();

        if (!animator)
        {
            Debug.Log("PlayerController: Failed to get Animator in parent");
        }

        coll = GetComponentInParent<Collider2D>();

        if (!coll)
        {
            Debug.Log("PlayerController: Failed to get Collider2D in parent");
        }
    }

    private bool wJump = false; // 记录跳跃键状态在下一逻辑帧更新

    // Update is called once per frame
    void Update()
    {
        // 检测跳跃键
        if (Input.GetButtonDown("Jump"))
        {
            wJump = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        SwitchAnim();
    }

    // 移动
    private void Move()
    {
        // 无法移动不处理
        if (!Moveable()) 
        {
            // 更新动画组件参数
            if (animator)
            {
                animator.SetFloat("speed", 0);
            }

            return;
        }

        // 获取输入
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");

        // 设置速度
        float horizontalSpeed = horizontalMove * mvoeSpeed * Time.fixedDeltaTime;
        float verticalSpeed = rb.velocity.y;

        // 根据跳跃键状态设置竖直速度
        if (wJump)
        {
            // 重置wJump
            wJump = false;
            verticalSpeed = jumpForce * Time.fixedDeltaTime;

            // 更新动画组件参数
            if (animator)
            {
                animator.SetBool("bJump", true);
            }
        }

        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        // 设置朝向
        if (faceDirection != 0)
        {
            Vector3 scale = rb.transform.localScale;
            float scaleX = faceDirection * Mathf.Abs(scale.x);

            rb.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        }

        // 更新动画组件参数
        if (animator)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontalSpeed));
        }
    }

    // 切换动画
    private void SwitchAnim()
    {
        if (!animator || !rb)
        {
            return;
        }

        // 跳跃中
        if (animator.GetBool("bJump"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("bJump", false);
                animator.SetBool("bFall", true);
            }
        }
        // 下落中
        else if (animator.GetBool("bFall"))
        {
            // TODO 暂时先根据速度来判断
            if (rb.velocity.y == 0)
            {
                animator.SetBool("bFall", false);
            }
        }
    }

    // 能否移动
    private bool Moveable() 
    {
        if (!rb)
        {
            return false;
        }

        return true;
    }
}
