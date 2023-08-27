using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cha3 : MonoBehaviour
{
    Animator animator;
    CharacterController cc;

    public float speed = 3;
    Vector3 move;

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

    void Move(float x, float y, float z)
    {
        move = new Vector3(x, 0, z);
        //这一帧移动的向量，很小
        Vector3 m = move * speed * Time.deltaTime;

        //朝向移动方向
        transform.LookAt(transform.position + m);

        //通过cc去移动
        cc.Move(m);
    }

    void UpdateAnim()
    {
        //基于刚体速度播放动画
        animator.SetFloat("Forward",cc.velocity.magnitude / speed);
    }
}

