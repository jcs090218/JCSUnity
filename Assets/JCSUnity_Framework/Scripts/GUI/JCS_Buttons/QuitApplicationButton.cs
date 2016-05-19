/**
 * $File: QuitApplication.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    public class QuitApplicationButton : JCS_Button
    {
        public override void JCS_ButtonClick()
        {
            JCS_ButtonFunctions.DestoryCurrentDialogue(JCS_DialogueType.SYSTEM_DIALOGUE);
            JCS_ButtonFunctions.QuitApplication();
        }
    }
}
