using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cha2 : MonoBehaviour
{
    Animator animator;
    Rigidbody rigid;

    public float speed = 3;
    Vector3 move;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
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

    void Move(float x, float y, float z)
    {
        //世界坐标
        move = new Vector3(x, y, z);
    }
    //刚体移动，要在FixedUpdate里写
    public void FixedUpdate()
    {
        //根据move，直接改变刚体速度
        Vector3 v = move * speed;
        v.y = rigid.velocity.y;
        rigid.velocity = new Vector3(move.x,rigid.velocity.y,move.z)* speed;

        //让刚体朝向目标
        Quaternion q = Quaternion.LookRotation(move);  //向量 转成 朝向
        rigid.MoveRotation(q);
    }

    void UpdateAnim()
    {
        float forward = move.magnitude;
        animator.SetFloat("Forward", forward);
    }
}

