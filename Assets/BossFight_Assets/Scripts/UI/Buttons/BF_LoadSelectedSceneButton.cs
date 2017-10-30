/**
 * $File: BF_LoadSelectedSceneButton.cs $
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
public class BF_LoadSelectedSceneButton 
    : JCS_Button 
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

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public override void JCS_OnClickCallback()
    {
        string sceneName = BF_GameSettings.instance.LEVEL_SELECTED_NAME;

        JCS_SceneManager.instance.LoadScene(sceneName);
    }

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
