/**
 * $File: RC_LevelSelectButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Use this button to select the level.
/// </summary>
public class RC_LevelSelectButton : JCS_Button 
{
    /* Variables */

    [SerializeField] private string mLevelName = "RC_Game";

    /* Setter & Getter */

    /* Functions */

    public override void OnClick()
    {
        var gs = RC_GameSettings.FirstInstance();

        gs.LEVEL_SELECTED_NAME = mLevelName;
        gs.SetCorrectSceneNameToAllButtonInScene();
    }
}
