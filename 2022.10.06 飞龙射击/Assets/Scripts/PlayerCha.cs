using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCha : MonoBehaviour
{
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 input)
    {
        transform.position += input * speed * Time.deltaTime;
    }
}
