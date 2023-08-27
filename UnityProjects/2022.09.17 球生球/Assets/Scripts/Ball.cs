using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigid;

    public float speed = 3;
    public float jumpPower = 1000;

    void Start()
    {
        Debug.Log(this.gameObject.name);
        Debug.Log(gameObject.name);
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //float s = Input.GetAxis("Jump");

        Vector3 input = new Vector3(h, 0, v);
        rigid.AddForce(input * speed);

        bool b1 = Input.GetButton("Jump");
        bool b2 = Input.GetButtonDown("Jump");
        bool b3 = Input.GetButtonUp("Jump");

        if (b1 || b2 || b3)
        {
            Debug.Log($" ‰»Î◊¥Ã¨:{b1} {b2} {b3}");
        }
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(new Vector3(0, jumpPower, 0));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("XXX");
        }

    }
}
