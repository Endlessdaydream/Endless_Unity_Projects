using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cha3Fall : MonoBehaviour
{
    Animator animator;
    CharacterController cc;

    public float speed = 3;
    [Range(0.0f, 1.0f)]
    public float testSpeed = 1;


    Vector3 move;

    bool isGround = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        //获取输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Move(h, 0, v);

        //更新动画
        UpdateAnim();
    }

    //纵向速度
    float vy = 0;
    void Move(float x, float y, float z)
    {
        move = new Vector3(x, 0, z);
        //这一帧移动的向量，很小
        Vector3 m = move * speed * Time.deltaTime;

        if (isGround)
        {
            vy = 0;
        }
        else
        {
            //物理公式：v = v0 + gt   (v0=0)
            vy += Physics.gravity.y * Time.deltaTime;
        }

        //Δy = v * Δt
        m.y = vy * Time.deltaTime;

        //朝向移动方向
        transform.LookAt(transform.position + move);

        //通过cc去移动
        cc.Move(m);
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.2f, 0), Vector3.down);

        RaycastHit hit;
        Color c = Color.red;

        isGround = false;
        if(Physics.Raycast(ray,out hit, 0.35f))
        {
            c = Color.green;
            isGround = true;
        }
        //调式
        Debug.DrawLine(transform.position + new Vector3(0, 0.2f, 0),transform.position - new Vector3(0,0.15f,0),c);
    }
    void UpdateAnim()
    {
        //基于刚体速度播放动画
        animator.SetFloat("Forward", cc.velocity.magnitude / speed * testSpeed);
    }
}

