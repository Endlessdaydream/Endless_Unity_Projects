using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 本脚本适用于纵向2D游戏的屏幕宽度匹配
// 如果是横向卷轴游戏，不需要用这个脚本
public class CamRatioAdapter : MonoBehaviour
{
    // 测试结果：屏幕宽高比3:4 = 0.75对应摄像机size3.75
    // 屏幕宽高比9:16 = 0.5625对应摄像机大小5

    [Tooltip("屏幕宽/高")]
    [SerializeField]
    float ratio1 = 0.75f;

    [Tooltip("正交摄像机Size")]
    [SerializeField]
    float size1 = 3.75f;

    Camera cam;

    void Update()
    {
        // 获取屏幕比例，计算正交摄像机的size
        float curRatio = (float)Screen.width / Screen.height;

        // 摄像机默认适配高度。当宽高比与摄像机size成反比时，能得到适配宽度的结果
        float a = ratio1 * size1;
        float size = a / curRatio;
        
        cam = GetComponent<Camera>();
        cam.orthographicSize = size;
    }

}
