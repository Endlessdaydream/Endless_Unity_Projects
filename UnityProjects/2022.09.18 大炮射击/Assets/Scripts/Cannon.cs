using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update
    public float hSpeed = 100;
    public float vSpeed = 60;

    public Rigidbody prefabBullet;
    public Transform cannonBody;
    public Transform bulletPos;
    public Transform pitch;  //¸©Ñö

    public float bulletSpeed = 50;

    void Start()
    {
        pitch = transform.Find("¸©Ñö");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //×óÓÒ
        transform.Rotate(v * vSpeed * Time.deltaTime, h * hSpeed * Time.deltaTime, 0);
        //¸©Ñö
        pitch.Rotate(v * vSpeed * Time.deltaTime,0,0);

        bool fire = Input.GetButtonDown("Jump");

        if (fire)
        {
            Rigidbody bullet = Instantiate (prefabBullet,bulletPos.position,transform.rotation);
            bullet.velocity = cannonBody.up * bulletSpeed;
        }
    }
}
