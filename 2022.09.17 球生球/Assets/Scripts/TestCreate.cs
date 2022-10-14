using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreate : MonoBehaviour
{
    // Start is called before the first frame update
    //预制体
    public GameObject prefabMouse;
    List<GameObject> objs;


    void Start()
    {
        objs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            for(int i = 0; i < 20; i++)
            {
                GameObject go = Instantiate(prefabMouse);
                go.transform.position = Random.insideUnitSphere * 3;

                objs.Add(go);
            }         
        }
        if (Input.GetButtonDown("Fire2"))
        {
            foreach(var go in objs)
            {
                if (!go.GetComponent<Rigidbody>())
                {
                    go.AddComponent<Rigidbody>();
                }                
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"List长度：{objs.Count}");
            foreach(var go in objs)
            {
                Destroy(go);
            }
            objs.Clear();
        }
    }
}
