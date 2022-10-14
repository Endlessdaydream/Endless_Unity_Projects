using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform t;//定义了放风筝的人
    Vector3 o;//定义了风筝线
    // Start is called before the first frame update
    void Start()
    {
        o = transform.position - t.position;//在游戏刚运行的时候确定好风筝线的长度、方向等信息
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = t.position + o;//让每时每刻风筝都被人和风筝线拖着走
                                            // 摄像机旋转
        float h = Input.GetAxis("Horizontal");
        float angle = 15 * h;
        Quaternion to = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, to, 0.05f);
    }
}
