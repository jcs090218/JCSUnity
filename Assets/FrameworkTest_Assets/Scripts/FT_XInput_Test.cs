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

    public bool valueKeyTest = true;

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
        PrintGamepadInfo(false);

        switch (JCS_InputSettings.instance.TargetGamePad)
        {
            case JCS_GamePadType.PS4:
                {
                    /* Stick test. */
                    if (valueKeyTest)
                    {
                        float val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_X);
                        print("Stick right X: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_Y);
                        print("Stick right Y: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_LEFT_X);
                        print("Stick left X: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_LEFT_Y);
                        print("Stick left Y: " + val);
                    }

                    if (JCS_Input.GetJoystickKeyDown(0, JCS_JoystickButton.BUTTON_A))
                        print("Joystick button Cir");
                    if (JCS_Input.GetJoystickKeyDown(0, JCS_JoystickButton.BUTTON_B))
                        print("Joystick button Sqr");
                    if (JCS_Input.GetJoystickKeyUp(0, JCS_JoystickButton.BUTTON_X))
                        print("Joystick button X");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_Y))
                        print("Joystick button Tri");

                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.LEFT_TRIGGER))
                        print("Left Trigger down");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.RIGHT_TRIGGER))
                        print("Right Trigger down");

                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.LEFT_BUMPER))
                        print("Left BUMPER down");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.RIGHT_BUMPER))
                        print("Right BUMPER down");

                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.START_BUTTON))
                        print("Options down");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BACK_BUTTON))
                        print("Share down");

                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_RIGHT))
                        print("Joystick button RIGHT");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_LEFT))
                        print("Joystick button LEFT");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_UP))
                        print("Joystick button UP");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_DOWN))
                        print("Joystick button DOWN");
                }
                break;

            case JCS_GamePadType.XBOX_360:
                {
                    /* Stick test. */
                    if (valueKeyTest)
                    {
                        float val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_X);
                        print("Stick right X: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_RIGHT_Y);
                        print("Stick right Y: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_LEFT_X);
                        print("Stick left X: " + val);

                        val = JCS_Input.GetAxis(0, JCS_JoystickButton.STICK_LEFT_Y);
                        print("Stick left Y: " + val);
                    }

                    if (JCS_Input.GetJoystickKeyDown(0, JCS_JoystickButton.BUTTON_A))
                        print("Joystick button A");
                    if (JCS_Input.GetJoystickKeyDown(0, JCS_JoystickButton.BUTTON_B))
                        print("Joystick button B");
                    if (JCS_Input.GetJoystickKeyUp(0, JCS_JoystickButton.BUTTON_X))
                        print("Joystick button X");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_Y))
                        print("Joystick button Y");


                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_RIGHT))
                        print("Joystick button RIGHT");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_LEFT))
                        print("Joystick button LEFT");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_UP))
                        print("Joystick button UP");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BUTTON_DOWN))
                        print("Joystick button DOWN");

                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.HOME_BUTTON))
                        print("Joystick button HOME");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.START_BUTTON))
                        print("Joystick button START");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.BACK_BUTTON))
                        print("Joystick button BACK");


                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.LEFT_BUMPER))
                        print("Left BUMPER down");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.RIGHT_BUMPER))
                        print("Right BUMPER down");


                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.LEFT_TRIGGER))
                        print("Right trigger down");
                    if (JCS_Input.GetJoystickButton(0, JCS_JoystickButton.RIGHT_TRIGGER))
                        print("Left trigger down");

                    if (JCS_Input.GetKeyDown(KeyCode.Joystick1Button0))
                    {
                        print(KeyCode.Joystick1Button0);
                    }
                }
                break;

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

    private void PrintGamepadInfo(bool fullInfo)
    {
        print("Joysitck connected: " + JCS_Input.IsJoystickConnected());

        if (!fullInfo)
            return;

        for (int index = 0;
            index < Input.GetJoystickNames().Length;
            ++index)
        {
            print("Index[" + index + "] => [" + Input.GetJoystickNames()[index] + "]");
        }
    }

}
