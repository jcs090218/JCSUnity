/** 
 * $File: ToSettingButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class ToSettingButton : JCS_Button
    {
        public override void JCS_ButtonClick()
        {
            //JCS_ButtonFunctions.PopSettingDialogue();

            JCS_GameWindowHandler.instance.GetPlayerDialogueAt(mDialogueIndex).ShowDialogue();
        }
    }
}
