using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    // Start is called before the first frame update
    //预制体
    public GameObject prefabMouse;
    List<GameObject> objs;

    void Start()
    {
        Debug.Log("Start" + Time.time);
        objs = new List<GameObject>();
    }
    
    void Create(int n)
    {
        for(int i = 0; i < n; i++)
        {
            GameObject go = Instantiate(prefabMouse);
            go.transform.position = Random.onUnitSphere * 10 + transform.position;

            objs.Add(go);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TestCoroutine(100));
        }
    }

    void DestroyAll()
    {
        foreach (var go in objs)
        {
            Destroy(go);
        }
        objs.Clear();
    } 

    //协程函数
    IEnumerator TestCoroutine(int n)
    {
        Create(n);

        yield return new WaitForSeconds(2);

        for(int i = 0; i < 400; i++)
        {
            foreach(var obj in objs)
            {
                //向delay物体所在位置运动
                Vector3 to = transform.position;
                //物体当前位置
                Vector3 from = obj.transform.position;
                Vector3 dir = to - from;
                obj.transform.position += dir.normalized * 3 * Time.deltaTime;

                //缩放
                obj.transform.localScale += new Vector3(2f, 2f, 2f) * Time.deltaTime;

            }
            yield return null;
        }

        DestroyAll();

    }
}
