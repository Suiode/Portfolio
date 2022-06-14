using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInvertToggle : MonoBehaviour
{
    public MouseLook mouse;
    public bool invertX = false;
    public bool invertY = false;

    public void ChangeInvertX()
    {
        if(invertX)
        {
            mouse.xMouseSens *= -1;
            invertX = true;
        }
        else
        {
            mouse.xMouseSens *= -1;
            invertX = false;
        }
        
    }

    public void ChangeInvertY()
    {
        if (invertY)
        {
            mouse.yMouseSens *= -1;
            invertY = true;
        }
        else
        {
            mouse.yMouseSens *= -1;
            invertY = false;
        }
    }
}
