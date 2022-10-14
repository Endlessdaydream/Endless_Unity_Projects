using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody r;
    public float speed = 10;
    public float m = 8;
    float p = 1.5f;
    float l = 0;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(z > 0)
        {
            speed = 15;
        }
        else if(z < 0)
        {
            speed = 5;
        }
        else
        {
            speed = 10;
        }


        r.velocity = new Vector3(x*m, r.velocity.y, speed);
        if(Time.time >= l + p)
        {
            Vector3 f = new Vector3(0, 4, 0);
            r.AddForce(f, ForceMode.Impulse);
            l = Time.time;
        }
    }
}
