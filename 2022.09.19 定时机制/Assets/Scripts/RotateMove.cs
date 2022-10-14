using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMove : MonoBehaviour
{
    public float speed = 3.0f;
    public float rotateSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //h控制旋转
        transform.Rotate(new Vector3(0, h * rotateSpeed * Time.deltaTime, 0));

        //v控制前进后退
        transform.Translate(new Vector3(0, 0, v) * speed * Time.deltaTime);


    }
}
