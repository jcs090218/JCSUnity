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
/// Load the scene base on the level selected 
/// by the player.
/// </summary>
public class BF_LoadSelectedSceneButton 
    : JCS_Button 
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    public override void JCS_OnClickCallback()
    {
        string sceneName = BF_GameSettings.instance.LEVEL_SELECTED_NAME;

        JCS_SceneManager.instance.LoadScene(sceneName);
    }
}
