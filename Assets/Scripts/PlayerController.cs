using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb; // 父组件的刚体组件

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
        }

        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        // 设置朝向
        if (faceDirection != 0)
        {
            Vector3 scale = rb.transform.localScale;
            float scaleX = faceDirection * Mathf.Abs(scale.x);

            rb.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        }
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
