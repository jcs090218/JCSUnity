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
        //float val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_X);
        //print(val);

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_A))
            print("Joystick button A");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_B))
            print("Joystick button B");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_X))
            print("Joystick button X");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_Y))
            print("Joystick button Y");


        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_RIGHT))
            print("Joystick right button down");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_LEFT))
            print("Joystick left button down");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_UP))
            print("Joystick button UP");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_DOWN))
            print("Joystick button DOWN");


        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.LEFT_TRIGGER))
            print("Right trigger down");

        if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.RIGHT_TRIGGER))
            print("Left trigger down");

        //if (JCS_Input.GetKeyDown(KeyCode.Joystick1Button0))
        //{
        //    print(KeyCode.Joystick1Button0);
        //}
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
