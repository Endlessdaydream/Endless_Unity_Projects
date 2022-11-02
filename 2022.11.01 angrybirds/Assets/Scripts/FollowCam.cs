using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    Vector2 offset;

    [HideInInspector]
    public bool isFollow = false;

    public Transform limit;
    Rect limitRect;

    void Start()
    {
        isFollow = false;
        offset = transform.position - target.position ;

        //计算限制范围的Rect
        limitRect.size = limit.localScale;
        limitRect.center = limit.position;

        Debug.Log("rect:{limitRect.xMin}{limitRect.xMax},{limitRect.yMin},limitRect.yMax}");
    }
    public void Follow()
    {
        Vector3 v = target.position + new Vector3(offset.x, offset.y, 0);
        v.z = -10;
        v.x = Mathf.Clamp(v.x, limitRect.xMin, limitRect.xMax);
        v.x = Mathf.Clamp(v.y, limitRect.yMin, limitRect.yMax);
        transform.position = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollow)
        {
            Vector3 v = target.position + new Vector3 (offset.x,offset.y,0);
            v.z = -10;

            //if (v.x < limitRect.xMin) { v.x = limitRect.xMin; }
            //if (v.x > limitRect.xMax) { v.x = limitRect.xMax; }
            //if (v.y < limitRect.yMin) { v.y = limitRect.yMin; }
            //if (v.y > limitRect.yMax) { v.y = limitRect.yMax; }

            //限定摄像头范围
            v.x = Mathf.Clamp(v.x, limitRect.xMin, limitRect.xMax);
            v.x = Mathf.Clamp(v.y, limitRect.yMin, limitRect.yMax);

            transform.position = v;
        }
    }
}
