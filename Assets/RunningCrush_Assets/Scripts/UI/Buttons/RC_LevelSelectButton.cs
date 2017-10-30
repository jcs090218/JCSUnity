/**
 * $File: RC_LevelSelectButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


/// <summary>
/// Use this button to select the level.
/// </summary>
public class RC_LevelSelectButton 
    : JCSUnity.JCS_Button 
{

    [SerializeField] private string mLevelName = "RC_Game";

    public override void JCS_OnClickCallback()
    {
        RC_GameSettings.instance.LEVEL_SELECTED_NAME = mLevelName;
        RC_GameSettings.instance.SetCorrectSceneNameToAllButtonInScene();
    }

}
