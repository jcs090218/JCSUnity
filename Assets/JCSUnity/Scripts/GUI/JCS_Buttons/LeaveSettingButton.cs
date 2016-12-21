/** 
 * $File: JCS_Button.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    public class LeaveSettingButton : JCS_Button
    {
        public override void JCS_ButtonClick()
        {
            JCS_UtilityFunctions.DestoryCurrentDialogue(JCS_DialogueType.PLAYER_DIALOGUE);
        }

    }
}
