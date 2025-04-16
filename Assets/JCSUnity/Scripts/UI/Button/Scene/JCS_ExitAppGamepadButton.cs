/**
 * $File: JCS_ExitAppGamepadButton.cs $
 * $Date: 2017-10-27 11:56:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Button exit the application. (Gamepad)
    /// </summary>
    public class JCS_ExitAppGamepadButton : JCS_GamepadButton
    {
        public override void OnClick()
        {
            // load exit button scene.
            JCS_SceneManager.instance.LoadScene("JCS_AppCloseSimulateOnline");
        }
    }
}
