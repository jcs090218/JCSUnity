/**
 * $File: npc2100.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class npc2100
        : JCS_DialogueScript
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        public override void Action(int mode, int type, int selection)
        {
            if (Status == -1)
                jcsDs.SendNext("好了. 看看一下你的能力值");
            else if (Status == 0)
                jcsDs.SendNextPrev("I found Abel's glasses.");
            else if (Status == 1)
                jcsDs.SendNextPrev("Okay~~ something here...");
            else if (Status == 2)
                jcsDs.SendYesNo("...");
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
