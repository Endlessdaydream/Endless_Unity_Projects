using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    PlayerCha player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerCha>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, v, 0);

        //调用角色的移动函数
        player.Move(move);

    }
}
