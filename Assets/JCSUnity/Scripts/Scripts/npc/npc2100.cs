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
            // if quest complete
            if (mode == -1)
                jcsDs.Dispose();

            if (Status == -1)
            {
                jcsDs.SendNext("好了. 看看一下你的能力值");
                jcsDs.SendNameTag("???");

                jcsDs.SendLeftImage(LeftSprite);
            }
            else if (Status == 0)
            {
                jcsDs.SendNextPrev("你可以選擇這些職業!");
                jcsDs.SendChoice(0, "劍士");
                jcsDs.SendChoice(1, "法師");
                jcsDs.SendChoice(2, "盜賊");
                jcsDs.SendChoice(3, "弓箭手");
                jcsDs.SendChoice(4, "海盜");
                jcsDs.SendChoice(5, "我再考慮看看...");

                jcsDs.SendNameTag("明日乂過後");

                jcsDs.SendRightImage(RightSprite);
            }
            else if (Status == 1)
            {
                string msg = "Okay~~ something here...";
                switch (selection)
                {
                    case 0:
                        msg = "你選擇了劍士";
                        break;
                    case 1:
                        msg = "你選擇了法師";
                        break;
                    case 2:
                        msg = "你選擇了盜賊";
                        break;
                    case 3:
                        msg = "你選擇了弓箭手";
                        break;
                    case 4:
                        msg = "你選擇了海盜";
                        break;
                    case 5:
                        msg = "你選擇了我再考慮看看...";
                        break;
                }
                jcsDs.SendNextPrev(msg);
            }
            else if (Status == 2)
                jcsDs.SendYesNo("...");
            else if (Status == 3)
            {
                string msg = "Default text";
                switch (selection)
                {
                    // Yes button clicked!
                    case 1:
                        msg = "You press Yes button!!! n(^O^)n";
                        break;
                    // No button clicked~
                    case 0:
                        msg = "You press No button!!! n(_ _)n";
                        break;
                }
                jcsDs.SendOk(msg);
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
