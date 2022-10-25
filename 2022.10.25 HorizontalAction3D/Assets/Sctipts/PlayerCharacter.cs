using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    Transform grabTrans;    //正在抓取的物体，没有物体为null
    public Transform grabPos;      //z抓取物体的点

    public void Grab()
    {
        if (grabTrans==null)
        {
            //尝试抓箱子
            Vector3 start = transform.position + new Vector3(0, 0.7f, 0);
            //Debug.DrawLine(start, start + transform.right, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(start, transform.right, out hit, 1, LayerMask.GetMask("GrabBox")))
            {
                Transform box = hit.transform;
                box.parent = grabPos;
                box.localPosition = Vector3.zero;
                box.localRotation = Quaternion.identity;
                box.GetComponent<Rigidbody>().isKinematic = true;
                grabTrans = box;
            }
        }
        else
        {
            //扔箱子
            grabTrans.parent = null;//将物体放回场景中（设置父物体为空，回到一级物体）
            grabTrans.GetComponent<Rigidbody>().isKinematic = false;
            grabTrans.position = new Vector3(grabTrans.position.x, grabTrans.position.y, 0);
            grabTrans.rotation = new Quaternion(grabTrans.rotation.x, 0, grabTrans.rotation.z, 0);
            grabTrans = null;
        }
    }

    private void Update()
    {
        if (grabTrans==null)
        {
            anim.SetBool("Grab", false);
        }
        else
        {
            anim.SetBool("Grab", true);
        }
    }
}
