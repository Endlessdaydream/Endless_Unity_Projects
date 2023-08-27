using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    Character character;
    float inputX;//模拟横向输入-1~1
    bool jump = false;

    public float changrActTime = 2;//改变行动的时间间隔
    public float jumpTime = 0.5f;//每个0.5秒尝试跳跃
    public float jumpChance = 30;//每次有30%概率跳跃
    void Start()
    {
        character = GetComponent<Character>();
        StartCoroutine(ChangeAct());
        StartCoroutine(AIJump());
    }

    // Update is called once per frame
    void Update()
    {
        character.Move(inputX, jump);
        jump = false;
    }

   IEnumerator ChangeAct()
    {
        while (true)
        {
            //改变inputX
            inputX = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(changrActTime);
        }
    }

    IEnumerator AIJump()
    {
        while (true)
        {
            int r = Random.Range(0, 100);
            if (r<jumpChance)
            {
                jump = true;
            }
            yield return new WaitForSeconds(jumpTime);
        }
    }

}
