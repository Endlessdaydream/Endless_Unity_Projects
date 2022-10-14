using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 to = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, to, 0.3f);
    }
}
