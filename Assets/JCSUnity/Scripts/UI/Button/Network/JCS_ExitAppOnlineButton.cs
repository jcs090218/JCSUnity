/**
 * $File: JCS_ExitAppOnlineButton.cs $
 * $Date: 2017-09-03 14:40:41 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Exit button with change scene effect.
    /// Multiplayer Version.
    /// </summary>
    public class JCS_ExitAppOnlineButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        public override void OnClick()
        {
            // load exit button scene.
            JCS_SceneManager.FirstInstance().LoadScene("JCS_AppCloseSimulateOnline");
        }
    }
}
