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
/// 
/// </summary>
public class FT_EchoGamePadButton 
    :  JCS_GamePadButton
{

    /// <summary>
    /// Default function to call this, so we dont have to
    /// search the function depends on name.
    /// 
    /// * Good for organize code and game data file in Unity.
    /// </summary>
    public override void JCS_ButtonClick()
    {
        base.JCS_ButtonClick();

        Debug.Log("echo.. Hello World!!");
    }
}
