using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerCharacter character;
    void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        character.Move(h,jump);

        if (Input.GetButtonDown("Fire1"))
        {
            character.Grab();
        }
    }
}
