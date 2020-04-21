/**
 * $File: FT_EchoGamePadButton.cs $
 * $Date: 2017-10-07 05:10:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;


/// <summary>
/// Test game pad button.
/// </summary>
public class FT_EchoGamePadButton 
    :  JCS_GamePadButton
{
    /* Variables */

    public JCS_DialogueObject closeDialogue = null;

    /* Setter/Getter */

    /* Functions */

    public override void JCS_OnClickCallback()
    {
        Debug.Log("echo.. Hello World!!");

        closeDialogue.HideDialogue();
    }
}
