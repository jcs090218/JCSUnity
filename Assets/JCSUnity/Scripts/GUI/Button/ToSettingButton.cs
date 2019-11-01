/**
 * $File: ToSettingButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Button will direct to settings panel.
    /// </summary>
    public class ToSettingButton 
        : JCS_Button
    {
        public override void JCS_OnClickCallback()
        {
            //JCS_UtilityFunctions.PopSettingDialogue();

            JCS_GameWindowHandler.instance.GetPlayerDialogueAt(mDialogueIndex).ShowDialogue();
        }
    }
}
