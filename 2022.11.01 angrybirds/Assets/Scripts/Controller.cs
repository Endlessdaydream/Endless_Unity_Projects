using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameMode.Instance.BeginDrag();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                GameMode.Instance.Drag(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameMode.Instance.EndDrag();
        }
    }
}
