/**
 * $File: FT_EchoGamepadButton.cs $
 * $Date: 2017-10-07 05:10:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Test game pad button.
/// </summary>
public class FT_EchoGamepadButton : JCS_GamepadButton
{
    /* Variables */

    public JCS_DialogueObject closeDialogue = null;

    /* Setter/Getter */

    /* Functions */

    public override void OnClick()
    {
        Debug.Log("echo.. Hello World!!");

        closeDialogue.Hide();
    }
}
