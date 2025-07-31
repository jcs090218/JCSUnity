/**
 * $File: BF_LoadSelectedSceneButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using JCSUnity;

/// <summary>
/// Load the scene base on the level selected 
/// by the player.
/// </summary>
public class BF_LoadSelectedSceneButton : JCS_Button
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    public override void OnClick()
    {
        string sceneName = BF_GameSettings.FirstInstance().LEVEL_SELECTED_NAME;

        JCS_SceneManager.FirstInstance().LoadScene(sceneName);
    }
}
