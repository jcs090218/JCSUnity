/**
 * $File: FT_XInput_Test.cs $
 * $Date: 2016-10-15 19:29:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class FT_XInput_Test
    : MonoBehaviour
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {

    }

    private void Update()
    {
        float val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_X);
        print(val);

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_RIGHT))
        {
            print("Right trigger down");
        }

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_LEFT))
        {
            print("Left trigger down");
        }

        if (JCS_Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            print(KeyCode.Joystick1Button0);
        }
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
