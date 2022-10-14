using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    bool jump;
    bool changeColor;
    PlayerCharacter cha;
    private void Start()
    {
        cha = GetComponent<PlayerCharacter>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            changeColor = true;
        }
    }
    private void FixedUpdate()
    {
        cha.Move(jump,changeColor);
        jump = false;
        changeColor = false;
        
    }
}
