/**
 * $File: JCS_ExitAppGamePadButton.cs $
 * $Date: 2017-10-27 11:56:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Button exit the app. (Game Pad)
    /// </summary>
    public class JCS_ExitAppGamePadButton
        : JCS_GamePadButton
    {
        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

            // load exit button scene.
            JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulateScene");
        }
    }
}
