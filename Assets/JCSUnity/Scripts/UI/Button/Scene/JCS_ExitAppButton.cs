/**
 * $File: JCS_ExitAppButton.cs $
 * $Date: 2017-05-04 02:14:59 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Button exit the application.
    /// </summary>
    public class JCS_ExitAppButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        public override void OnClick()
        {
            // load exit button scene.
            JCS_SceneManager.FirstInstance().LoadScene("JCS_AppCloseSimulate");
        }
    }
}
