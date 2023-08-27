using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject prefabBall;
    List<GameObject> objs;
    void Start()
    {
        objs = new List<GameObject>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go = Instantiate(prefabBall);
            go.transform.position = Random.insideUnitSphere * 3;

            objs.Add(go);
        }
    }
}
