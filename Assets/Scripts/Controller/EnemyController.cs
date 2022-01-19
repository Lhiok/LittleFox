using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorExtensions;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("属性")]
    public ItemEnum type;  // 怪物类型
    public int hp;  // 血量
    public int atk; // 攻击力
    public float bounceForce;   // 击飞力

    [Header("特效")]
    public GameObject fxEnemyDeath; 

    protected int curHp;  // 剩余血量

    // Start is called before the first frame update
    void Start()
    {
        // 初始化参数
        curHp = hp;
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        // 触碰到玩家
        if (other.gameObject.tag == "Player")
        {
            Animator playerAnim = other.gameObject.GetComponent<Animator>();
            // 被玩家踩到受伤
            if (playerAnim.GetBool("bFall") && other.transform.position.y > this.transform.position.y)
            {
                --curHp;
                // 将玩家弹飞
                EventUtil.Dispatch(EventEnum.Player_Bounce, new Vector2(0, bounceForce));

                // 血量归零死亡
                if (curHp == 0)
                {
                    Destroy(this.gameObject);
                    EventUtil.Dispatch(EventEnum.Kill_Enemy, type);
                    EventUtil.Dispatch(EventEnum.Fx_Play_Once, fxEnemyDeath, transform);
                }
                return;
            }

            // 计算受力方向
            Vector2 dir = (other.transform.position - this.transform.position).XY().normalized;

            // 触碰玩家导致其受伤
            EventUtil.Dispatch(EventEnum.Player_Hurt, atk, dir * bounceForce);
        }
    }
}
