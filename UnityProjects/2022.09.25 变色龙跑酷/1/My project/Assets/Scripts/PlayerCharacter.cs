using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerColor
{
    Red,
    Green,
}

public class PlayerCharacter : MonoBehaviour
{
    //各种组件
    Rigidbody rigid;
    Animator anim;
    Renderer render;
    public float speed = 10;
    public float jumpSpeed = 4.6f;
    int jumpCount = 0;

    
    bool isGround;

    PlayerColor color = PlayerColor.Red;

    public Transform prefabDieParticleRed;
    public Transform prefabDieParticleGreen;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        render = GetComponentInChildren<Renderer>();
        Debug.Log("render" + render);
    }

    // Update is called once per frame

    public void Move(bool jump,bool changeColor)
    {
        Vector3 vel = rigid.velocity;
        vel.z = speed;
        if (jump && jumpCount < 10)
        {
            vel.y = jumpSpeed;
            jumpCount++;
        }
        rigid.velocity = vel;
        anim.SetBool("IsGround", isGround);
        isGround = false;

        if (changeColor)
        {
            ChangeColor();
        }
        
    }

    void ChangeColor()
    {
        if(color == PlayerColor.Red)
        {
            color = PlayerColor.Green;
        }
        else
        {
            color = PlayerColor.Red;
        }
        if (color == PlayerColor.Red)
        {
            render.material.color = Color.red;
        }
        else
        {
            render.material.color = Color.green;
        }
        anim.SetTrigger("Change");
    }
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if(tag == "Green"||tag == "Red")
        {
            jumpCount = 0;
            isGround = true;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Green" || tag == "Red")
        {
            jumpCount = 0;
            isGround = true;
        }

        if(color == PlayerColor.Green && tag == "Red")
        {
            PlayerDie();
        }
        else if(color == PlayerColor.Red && tag == "Green")
        {
            PlayerDie();
        }
    }
    void PlayerDie()
    {
        gameObject.SetActive(false);
        //播放爆炸粒子
        Transform prefab = prefabDieParticleRed;
        if(color == PlayerColor.Green)
        {
            prefab = prefabDieParticleGreen;
        }
        Transform p = Instantiate(prefabDieParticleRed,transform.position,Quaternion.identity);

        Invoke("DelayPlayerDie", 1);
    }

    void DelayPlayerDie()
    {
        //调用GameMode的OnPlayerDie函数
        GameMode.Instance.OnPlayerDie();
    }

}
