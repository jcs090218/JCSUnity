/**
 * $File: BF_SelectLevelButton.cs $
 * $Date: $
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
public class BF_SelectLevelButton 
    : JCS_Button
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [Tooltip("Name of the scene u want to load for game.")]
    [SerializeField]
    private string mSceneName = "";

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public override void JCS_ButtonClick()
    {
        base.JCS_ButtonClick();

        BF_GameSettings.instance.LEVEL_SELECTED_NAME = mSceneName;
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
