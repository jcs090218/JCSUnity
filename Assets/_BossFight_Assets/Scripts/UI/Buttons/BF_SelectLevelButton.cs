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
    /* Variables */

    [Tooltip("Name of the scene you want to load for game.")]
    [SerializeField]
    private string mSceneName = "";

    /* Setter & Getter */

    /* Functions */

    public override void JCS_OnClickCallback()
    {
        BF_GameSettings.instance.LEVEL_SELECTED_NAME = mSceneName;
    }
}
