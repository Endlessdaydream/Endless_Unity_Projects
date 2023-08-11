using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(Loop());
        //StartCoroutine(Loop());
        StartCoroutine(Logic(10));
        //StartCoroutine(Simple(100));
    }

    IEnumerator Simple(int n)
    {
        Debug.Log(11111);
        yield return null;

        for (int i=0; i<10; i++)
        {
            Debug.Log(22222);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Logic(int n)
    {
        yield return new WaitForSeconds(0.1f);
        if (n == 0)
        {
            yield break;
        }

        Debug.Log(n + " " + Time.time);

        Logic(n - 1);
    }

    IEnumerator Logic2()
    {
        Debug.Log("Logic2");
        yield return null;
    }

    // 迭代器
    IEnumerator Loop()
    {
        Debug.Log("A");

        yield return new WaitForSeconds(2);

        Debug.Log("B");
        yield return new WaitForSeconds(1);


        Debug.Log("C");

        while (!Input.GetButton("Jump"))
        {
            yield return null;
        }
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("中止");
            yield break;
        }

        Debug.Log("DDDD");
    }

}
