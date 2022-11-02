using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonType 
{
    horizontal,
    vertical,
    jump,
    action,
}
public class VirtualContorller : MonoBehaviour
{
    public static VirtualContorller virtualContorller;

    static FixedJoystick fixedJoystick;

    bool isJump;
    bool isAction;

    void Awake()
    {
        virtualContorller = this;
    }

    void Start()
    {
        //获取虚拟摇杆
        fixedJoystick = transform.GetChild(0).GetComponent<FixedJoystick>();

        //跳跃按钮事件添加
        EventTrigger eventTrigger1 = transform.GetChild(1).GetComponent<EventTrigger>();
        EventTrigger.Entry click1 = new EventTrigger.Entry();
        click1.eventID = EventTriggerType.PointerDown;
        click1.callback.AddListener(JumpButtonDownEvent);

        eventTrigger1.triggers.Add(click1);

        //行动按钮事件添加
        EventTrigger eventTrigger2 = transform.GetChild(2).GetComponent<EventTrigger>();
        EventTrigger.Entry click2 = new EventTrigger.Entry();
        click2.eventID = EventTriggerType.PointerDown;
        click2.callback.AddListener(ActionButtonDownEvent);
        eventTrigger2.triggers.Add(click2);
    }

    void LateUpdate()
    {
        isJump = false;
        isAction = false;
    }

    public static bool GetButtonDown(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.jump:
                return virtualContorller.isJump;

            case ButtonType.action:
                return virtualContorller.isAction;

            default:
                return false;
        }
    }

    public static float GetAxis(ButtonType type)
    {
        switch(type)
        {
            case ButtonType.horizontal:
                return fixedJoystick.Horizontal;
            case ButtonType.vertical:
                return fixedJoystick.Vertical;
        }
        return 0;
    }

    public void JumpButtonDownEvent(BaseEventData data)
    {
        isJump = true;
    }
    public void ActionButtonDownEvent(BaseEventData data)
    {
        isAction = true;
    }
}