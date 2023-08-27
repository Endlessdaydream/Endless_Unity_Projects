using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        if (input.magnitude > 1)
        {
            input = input.normalized;
        }

        //考虑时间，抵消帧率变化的影响
        //transform.position += input * speed * Time.deltaTime;         //世界坐标系
        transform.Translate(input * speed * Time.deltaTime,Space.World);//世界坐标系
        transform.Translate(input * speed * Time.deltaTime);            //自身坐标系
        transform.Translate(input * speed * Time.deltaTime,Space.Self); //自身坐标系
    }
}
