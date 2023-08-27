using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBirds : MonoBehaviour
{
    bool dead = false;
    Rigidbody2D rigid;
    float startFlyTime = 0;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead)
        {
            Debug.Log("小鸟碰到了" + collision.collider.name);
            if (startFlyTime + 0.1f < Time.time)
            {
                Invoke("Die", 2.5f);
                dead = true;
            }
        }
    }
    private void Update()
    {
        //如果小鸟越界，则死亡
        if(transform.position.x > 25 || transform.position.x < -25 || transform.position.y > 30 || transform.position.y < -30)
        {
            Invoke("Die", 2.5f);
            dead = true;
        }
    }
    void Die()
    {
        GameMode.Instance.OnPlayerBirdDie();
    }
    public void StartFly()
    {
        startFlyTime = Time.time;
    }
    public void ResetBird()
    {
        rigid.isKinematic = true;
        //小鸟旋转归0
        transform.rotation = Quaternion.identity;
        //rigid.rotation = 0;  //作用与上面一句等价

        //小鸟速度归0
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        dead = false;
    }
}
