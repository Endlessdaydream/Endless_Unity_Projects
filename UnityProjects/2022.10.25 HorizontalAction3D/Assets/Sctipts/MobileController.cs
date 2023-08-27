using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{

    PlayerCharacter character;
    void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = VirtualContorller.GetAxis(ButtonType.horizontal);
        bool jump = VirtualContorller.GetButtonDown(ButtonType.jump);

        character.Move(h,jump);

        if (VirtualContorller.GetButtonDown(ButtonType.action))
        {
            character.Grab();
        }
    }

   

}
